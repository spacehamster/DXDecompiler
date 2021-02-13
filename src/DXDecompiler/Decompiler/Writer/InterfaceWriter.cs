using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Decompiler.IR;
using System;
using System.Linq;

namespace DXDecompiler.Decompiler.Writer
{
	public class InterfaceWriter : BaseWriter
	{
		public InterfaceWriter(DecompileContext context) : base(context)
		{

		}
		public void WriteInterface(IrInterfaceManger manager)
		{
			WriteFormat(manager.Chunk.ToString());
			WriteInterfacePointers(manager);
		}
		void WriteInterfacePointers(IrInterfaceManger manager)
		{
			var interfaces = manager.Chunk;
			var rdef = Context.Shader.ResourceDefinition.Chunk;
			WriteLine("// Interface Pointers");
			foreach(var cb in rdef.ConstantBuffers
				.Where(cb => cb.BufferType == ConstantBufferType.InterfacePointers))
			{
				WriteLine($"{cb.ToString()}");
			}
			var cbVariables = rdef.ConstantBuffers
				.Where(cb => cb.BufferType == ConstantBufferType.InterfacePointers)
				.Single()
				.Variables;
			foreach(var group in cbVariables.GroupBy(v => GetShaderTypeName(v.ShaderType)))
			{
				var itf = group.First();
				WriteLine($"interface {group.Key} ");
				WriteLine("{");

				var slots = interfaces.InterfaceSlots.Single(i => i.StartSlot == itf.StartOffset);
				var tableId = slots.TableIDs.First();
				var functionTables = manager.FunctionTables[$"ft{tableId}"];
				for(int i = 0; i < functionTables.Count; i++)
				{
					WriteLine($"    void Call{i}();");
				}
				WriteLine("};");
			}
			foreach(var variable in cbVariables)
			{
				// interface definition must come before instance
				var indexString = variable.ShaderType.ElementCount > 0 ? $"[{variable.ShaderType.ElementCount}]" : "";
				WriteLine($"{GetShaderTypeName(variable.ShaderType)} {variable.Name}{indexString};");
			}
			foreach(var ct in interfaces.AvailableClassTypes)
			{
				var slots = interfaces.InterfaceSlots.First(i => i.TypeIDs.Contains(ct.ID));
				var itf = cbVariables.First(v => v.StartOffset == slots.StartSlot);
				WriteLine($"class {ct.Name} : {GetShaderTypeName(itf.ShaderType)}");
				WriteLine("{");
				IncreaseIndent();
				var functionTables = manager.FunctionTables[$"ft{ct.ID}"];
				WriteIndent();
				WriteLine($"// ft{ct.ID}");
				WriteIndent();
				WriteLine($"{slots}");
				for(int i = 0; i < functionTables.Count; i++)
				{
					var functionBody = manager.Passes[functionTables[i]];
					WriteIndent();
					WriteLine($"void Call{i}(){{");
					IncreaseIndent();
					foreach(var token in functionBody.Instructions)
					{
						Context.InstructionWriter.WriteInstruction(token);
					}
					DecreaseIndent();
					WriteIndent();
					WriteLine($"}}");
				}
				DecreaseIndent();
				WriteLine("};");
			}
		}
		internal string GetShaderTypeName(ShaderType variable)
		{
			switch(variable.VariableClass)
			{
				case ShaderVariableClass.InterfacePointer:
					{
						if(!string.IsNullOrEmpty(variable.BaseTypeName)) // BaseTypeName is only populated in SM 5.0
						{
							return variable.BaseTypeName;
						}
						else
						{
							return string.Format("{0}{1}",
								variable.VariableClass.GetDescription(),
								variable.VariableType.GetDescription());
						}
					}
				default:
					throw new ArgumentException(variable.ToString());
			}
		}
	}
}
