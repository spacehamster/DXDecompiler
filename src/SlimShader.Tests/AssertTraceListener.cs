using NUnit.Framework;
using System.Diagnostics;

namespace SlimShader.Tests
{
	class AssertTraceListener : TraceListener
	{
		public static void Init()
		{
			Debug.Listeners.Clear();
			Debug.Listeners.Add(new AssertTraceListener());
		}
		public override void Fail(string msg, string detailedMsg)
		{
			string message = string.IsNullOrEmpty(msg) ? "No message" : msg;
			Assert.Fail($"Assetion failed: {message}");
		}

		public override void Write(string message)
		{

		}

		public override void WriteLine(string message)
		{

		}
	}

}
