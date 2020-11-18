using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	public enum EffectCompilerAssignmentType
	{
		Invalid,
		Constant,
		Variable,
		ConstantIndex,
		VariableIndex,
		ExpressionIndex,
		Expression,
		InlineShader,
		InlineShader5
	}
}
