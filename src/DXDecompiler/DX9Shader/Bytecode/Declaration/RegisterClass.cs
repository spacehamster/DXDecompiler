using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader
{
	// D3DXPARAMETER_CLASS
	public enum ParameterClass
	{
		Scalar,
		Vector,
		MatrixRows,
		MatrixColumns,
		Object,
		Struct
	}
}
