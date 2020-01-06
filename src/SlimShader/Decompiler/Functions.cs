using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using System.IO;
using System.Text;
using System.Linq;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Chunks.Rdef;
using System;
using SlimShader.Chunks;

namespace SlimShader.Decompiler
{
	public class Functions
	{
		BytecodeContainer Container;
		StringBuilder Output = new StringBuilder();
		Dictionary<string, List<OpcodeToken>> FunctionBodies = new Dictionary<string, List<OpcodeToken>>();
		Dictionary<string, List<string>> FunctionTables = new Dictionary<string, List<string>>();
		public Functions(BytecodeContainer container)
		{
			Container = container;
			List<OpcodeToken> currentBody = null;
			foreach (var token in container.Shader.Tokens)
			{
				switch(token.Header.OpcodeType)
				{
					case OpcodeType.DclFunctionBody:
						break;
					case OpcodeType.DclFunctionTable:
						{
							var dcl = token as FunctionTableDeclarationToken;
							FunctionTables[$"ft{dcl.Identifier}"] = dcl.FunctionBodyIndices
								.Select(i => $"fb{i}")
								.ToList();
							break;
						}
					case OpcodeType.DclInterface:
						break;
					case OpcodeType.Label:
						{
							var inst = token as InstructionToken;
							var operand = inst.Operands[0];
							currentBody = new List<OpcodeToken>();
							currentBody.Add(token);
							FunctionBodies[operand.ToString()] = currentBody;
							break;
						}
					default:
						if(currentBody != null)
						{
							currentBody.Add(token);
						}
						break;
				}
			}
			//TODO: Merge Duplicate function bodies
		}
		public Dictionary<string, string> GetRegisterMapping()
		{
			var result = new Dictionary<string, string>();
			var cbVariables = Container.ResourceDefinition.ConstantBuffers
				.Where(cb => cb.BufferType == ConstantBufferType.InterfacePointers)
				.Single()
				.Variables;
			foreach (var intf in cbVariables)
			{
				var elementCount = intf.ShaderType.ElementCount;
				if (elementCount == 0) elementCount = 1;
				elementCount = 1; //Disable element indexing
				var slots = Container.Interfaces.InterfaceSlots.Single(slot => slot.StartSlot == intf.StartOffset);
				var funtionTable = FunctionTables[$"ft{slots.TableIDs.First()}"];
				for (int i = 0; i < elementCount; i++)
				{
					var indexString = elementCount >= 2 ? $"[{i}]" : "";
					for (int j = 0; j < funtionTable.Count; j++)
					{
						result[$"fp{intf.StartOffset}{indexString}[{j}]"] = $"{intf.Name}{indexString}.Call{j}";
					}
				}
			}
			return result;
		}
		public string Dump()
		{
			WriteInterfaces();
			WriteInterfacePointers();
			return Output.ToString();
		}
		internal string GetShaderTypeName(ShaderType variable)
		{
			switch (variable.VariableClass)
			{
				case ShaderVariableClass.InterfacePointer:
					{
						if (!string.IsNullOrEmpty(variable.BaseTypeName)) // BaseTypeName is only populated in SM 5.0
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
		void WriteInterfacePointers()
		{
			Output.AppendLine("// Interface Pointers");
			foreach (var cb in Container.ResourceDefinition.ConstantBuffers
				.Where(cb => cb.BufferType == ConstantBufferType.InterfacePointers))
			{
				Output.AppendLine($"{cb.ToString()}");
			}
			var cbVariables = Container.ResourceDefinition.ConstantBuffers
				.Where(cb => cb.BufferType == ConstantBufferType.InterfacePointers)
				.Single()
				.Variables;
			foreach (var group in cbVariables.GroupBy(v => GetShaderTypeName(v.ShaderType)))
			{
				var itf = group.First();
				Output.AppendLine($"interface {group.Key} ");
				Output.AppendLine("{");

				var slots = Container.Interfaces.InterfaceSlots.Single(i => i.StartSlot == itf.StartOffset);
				var tableId = slots.TableIDs.First();
				var functionTables = FunctionTables[$"ft{tableId}"];
				for (int i = 0; i < functionTables.Count; i++)
				{
					Output.AppendLine($"    void Call{i}();");
				}					
				Output.AppendLine("};");
			}
			foreach (var variable in cbVariables)
			{
				// interface definition must come before instance
				var indexString = variable.ShaderType.ElementCount > 0 ? $"[{variable.ShaderType.ElementCount}]" : "";
				Output.AppendLine($"{GetShaderTypeName(variable.ShaderType)} {variable.Name}{indexString};");
			}
			foreach (var ct in Container.Interfaces.AvailableClassTypes)
			{
				var slots = Container.Interfaces.InterfaceSlots.First(i => i.TypeIDs.Contains(ct.ID));
				var itf = cbVariables.First(v => v.StartOffset == slots.StartSlot);
				Output.AppendLine($"class {ct.Name} : {GetShaderTypeName(itf.ShaderType)}");
				Output.AppendLine("{");
				var functionTables = FunctionTables[$"ft{ct.ID}"];
				Output.AppendLine($"    // ft{ct.ID}");
				Output.AppendLine($"{slots}");
				for (int i = 0; i < functionTables.Count; i++)
				{
					var functionBody = FunctionBodies[functionTables[i]];
					Output.AppendLine($"    void Call{i}(){{");
					foreach(var token in functionBody)
					{
						Output.AppendLine($"        // {token.ToString()}");
					}
					Output.AppendLine($"    }}");

				}
				Output.AppendLine("};");
			}
		}
		void WriteInterfaces()
		{
			var interfaces = Container.Interfaces;
			if (interfaces == null) return;
			Output.AppendFormat(Container.Interfaces.ToString());
		}
	}
}
