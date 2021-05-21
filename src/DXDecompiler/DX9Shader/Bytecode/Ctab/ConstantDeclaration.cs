using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader.Bytecode.Ctab
{
	/// <summary>
	/// Refer D3DXSHADER_CONSTANTINFO d3dx9shader.h
	/// https://docs.microsoft.com/en-us/windows/win32/direct3d9/d3dxshader-constantinfo
	/// Note also D3DXCONSTANT_DESC d3dx9shader.h which is a combined ConstantInfo and ConstantType struct
	/// https://docs.microsoft.com/en-us/windows/win32/direct3d9/d3dxconstant-desc
	/// </summary>
	public class ConstantDeclaration
	{
		public string Name { get; private set; }
		public RegisterSet RegisterSet { get; private set; }
		public ushort RegisterIndex { get; private set; }
		public ushort RegisterCount { get; private set; }
		public ConstantType Type { get; private set; }
		public List<float> DefaultValue { get; private set; }

		public uint Rows => Type.Rows;
		public uint Columns => Type.Columns;
		public ParameterType ParameterType => Type.ParameterType;
		public ParameterClass ParameterClass => Type.ParameterClass;
		public uint Elements => Type.Elements;

		public ConstantDeclaration(string name, RegisterSet registerSet, short registerIndex, short registerCount,
			ParameterClass parameterClass, ParameterType parameterType, int rows, int columns, int elements, List<float> defaultValue)
		{
			Name = name;
			RegisterSet = registerSet;
			RegisterIndex = (ushort)registerIndex;
			RegisterCount = (ushort)registerCount;
			Type = new ConstantType()
			{
				ParameterClass = parameterClass,
				ParameterType = parameterType,
				Rows = (uint)rows,
				Columns = (uint)columns,
				Elements = (uint)elements,
			};
			DefaultValue = defaultValue;
		}
		public ConstantDeclaration()
		{
			DefaultValue = new List<float>();
		}
		public static ConstantDeclaration Parse(BytecodeReader reader, BytecodeReader decReader)
		{
			var result = new ConstantDeclaration();
			var nameOffset = decReader.ReadUInt32();
			result.RegisterSet = (RegisterSet)decReader.ReadUInt16();
			result.RegisterIndex = decReader.ReadUInt16();
			result.RegisterCount = decReader.ReadUInt16();
			decReader.ReadUInt16(); //Reserved
			var typeInfoOffset = decReader.ReadUInt32();
			var defaultValueOffset = decReader.ReadUInt32();

			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();

			var typeReader = reader.CopyAtOffset((int)typeInfoOffset);
			result.Type = ConstantType.Parse(reader, typeReader);

			if(defaultValueOffset != 0)
			{
				//Note: thre are corrisponding def instructions. TODO: check that they are the same
				var defaultValueReader = reader.CopyAtOffset((int)defaultValueOffset);
				for(int i = 0; i < 4; i++)
				{
					result.DefaultValue.Add(defaultValueReader.ReadSingle());
				}
			}
			return result;
		}
		public bool ContainsIndex(uint index)
		{
			return (index >= RegisterIndex) && (index < RegisterIndex + RegisterCount);
		}

		public override string ToString()
		{
			return Name;
		}
		public string GetTypeName()
		{
			return Type.GetTypeName();
		}
		public string GetRegisterName()
		{
			return $"{RegisterSet.GetDescription()}{RegisterIndex}";
		}

		public string GetConstantNameByRegisterNumber(uint registerNumber, string relativeAddressing)
		{
			var decl = this;
			var totalOffset = registerNumber - decl.RegisterIndex;
			var data = decl.GetRegisterTypeByOffset(totalOffset);
			var name = decl.GetMemberNameByOffset(totalOffset);
			var offsetFromMember = registerNumber - data.RegisterIndex;

			if(relativeAddressing != null)
			{
				// a nasty way to check array subscripts
				// TODO: we need something more serious...
				if(decl.Elements <= 1 || !name.EndsWith("]") || name.Count(x => x == ']') != 1)
				{
					// we cannot handle relative addressing if this contant declaration is not an array
					// we also cannot handle relative addressing if this constant declaration contains nested arrays
					name = $"Error({name}, {relativeAddressing})";
				}
				else
				{
					var declElementSize = decl.RegisterCount / decl.Elements;
					if(declElementSize > 1)
					{
						relativeAddressing = $"({relativeAddressing} / {declElementSize})";
					}
					name = name.Substring(0, name.Length - 1) // name without the last ']'
						+ $" + {relativeAddressing}]";
				}
			}

			// how many registers should be occupied by the current constant item
			var registersOccupied = data.Type.ParameterClass == ParameterClass.MatrixColumns
				? data.Type.Columns
				: data.Type.Rows;
			// sanity check: if the current constant item only occupies one register
			// then its start index must match with the register number
			if(registersOccupied == 1 && data.RegisterIndex != registerNumber)
			{
				throw new InvalidOperationException();
			}

			// matrix row / columns
			if(data.Type.ParameterClass == ParameterClass.MatrixRows)
			{
				return $"{name}[{offsetFromMember}]";
			}
			else if(data.Type.ParameterClass == ParameterClass.MatrixColumns)
			{
				var accessors = Enumerable.Range(0, (int)data.Type.Rows).Select(i => $"m{i}{offsetFromMember}");
				return $"({name}._{string.Join("_", accessors)})";
			}

			return name;
		}

		public class ConstantRegisterData
		{
			public ConstantType Type { get; }
			public uint RegisterIndex { get; }

			public ConstantRegisterData(ConstantType type, uint registerIndex)
			{
				Type = type;
				RegisterIndex = registerIndex;
			}
		}
		/// <summary>
		/// Get the information of register by an offset, 
		/// relative to the register index of this const declaration.
		/// </summary>
		/// <param name="offset">The offset, relative to this const declaration's initial address.</param>
		/// <returns>Information about the specified register.</returns>
		public ConstantRegisterData GetRegisterTypeByOffset(uint offset)
		{
			if(offset > RegisterCount)
			{
				throw new ArgumentOutOfRangeException();
			}

			var originalOffset = offset;
			// keep track of the last visited type, which is the type of member being found
			ConstantType lastType = null;
			TraverseChildTree(ref offset, (type, name, i, d) =>
			{
				lastType = type;
			});

			// sanity check
			if(lastType == null)
			{
				throw new InvalidOperationException();
			}

			// the offset of member, relative to the constant declaration's initial address
			var memberOffset = originalOffset - offset;

			return new ConstantRegisterData(lastType, RegisterIndex + memberOffset);
		}

		/// <summary>
		/// Get the name of member variable by offset, 
		/// relative to the register index of this const declaration.
		/// </summary>
		/// <param name="offset">The offset, relative to this const declaration's initial address.</param>
		/// <returns>The member variable name.</returns>
		public string GetMemberNameByOffset(uint offset)
		{
			if(offset > RegisterCount)
			{
				throw new ArgumentOutOfRangeException();
			}
			var indexes = new List<(string Name, uint Index, uint MaxIndex)> { (Name, 0, 0) };
			var lastDepth = 0;
			TraverseChildTree(ref offset, (type, name, index, depth) =>
			{
				if(depth >= indexes.Count)
				{
					indexes.Add((name, index, type.Elements));
				}
				else
				{
					indexes[depth] = (name, index, type.Elements);
				}
				lastDepth = depth;
			});
			// build the string
			var result = string.Empty;
			foreach(var (name, i, max) in indexes.Take(lastDepth + 1))
			{
				if(max > 1)
				{
					// we are accessing array elements
					result += $"{name}[{i}].";
				}
				else
				{
					result += $"{name}.";
				}
			}
			return result.TrimEnd('.');
		}

		private delegate void ChildTreeVisitor(ConstantType type, string name, uint index, int depth);

		/// <summary>
		/// Traverse the child tree of target variable, until a child matching the specified 
		/// register offset has been found.
		/// This might be useful when the target is a struct or an array.
		/// </summary>
		/// <param name="type">The type of current target.</param>
		/// <param name="offset">
		/// The offset of child element to be searched.
		/// It will become 0 if a child element has been found exactly on the specified offset,
		/// non-zero if we are accessing "in the middle of an element",
		/// something like "accessing the 2nd column of matrix".
		/// </param>
		/// <param name="visitor">
		/// The visitor will be called with child element type and current depth.
		/// </param>
		private void TraverseChildTree(ref uint offset, ChildTreeVisitor visitor)
		{
			var found = TraverseChildTree(Name, Type, ref offset, visitor, 0);
			if(!found)
			{
				throw new InvalidOperationException();
			}
		}
		private static bool TraverseChildTree(
			string name,
			ConstantType type,
			ref uint remainedOffset,
			ChildTreeVisitor visitor,
			int depth)
		{
			for(var i = 0u; i < type.Elements; i++)
			{
				visitor(type, name, i, depth);

				uint registersOccupied;
				switch(type.ParameterClass)
				{
					case ParameterClass.Struct:
						if(!type.Members.Any())
						{
							throw new NotImplementedException();
						}
						foreach(var member in type.Members)
						{
							var found = TraverseChildTree(member.Name, member.Type, ref remainedOffset, visitor, depth + 1);
							if(found)
							{
								return true;
							}
						}
						continue;
					case ParameterClass.Object:
						// sanity check
						if(type.Members.Any())
						{
							throw new InvalidOperationException();
						}
						if(remainedOffset == 0)
						{
							return true;
						}
						remainedOffset -= 1;
						continue;
					case ParameterClass.MatrixColumns:
					case ParameterClass.MatrixRows:
					case ParameterClass.Vector:
					case ParameterClass.Scalar:
						// sanity check
						if(type.Members.Any())
						{
							throw new InvalidOperationException();
						}
						registersOccupied = type.ParameterClass == ParameterClass.MatrixColumns
							? type.Columns
							: type.Rows;
						break;
					default:
						throw new NotImplementedException();
				}

				// ParameterClass.MatrixColumns / ParameterClass.Rows
				// ParameterClass.Vector
				// ParameterClass.Scalar:
				switch(type.ParameterType)
				{
					case ParameterType.Bool:
					case ParameterType.Float:
					case ParameterType.Int:
						// double is not supported for now as it occupies more than 1 register
						// but it's not inside ParameterType?
						break;
					default:
						throw new NotImplementedException();
				}
				if(remainedOffset < registersOccupied)
				{
					return true;
					// here, remainedOffset intentionally left unchanged when returning true.
				}
				remainedOffset -= registersOccupied;
			}
			return false;
		}
	}
}
