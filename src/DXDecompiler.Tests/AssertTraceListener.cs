using NUnit.Framework;
using System.Diagnostics;

namespace DXDecompiler.Tests
{
	class AssertTraceListener : TraceListener
	{
		public static void Init()
		{
			Trace.Listeners.Clear();
			Trace.Listeners.Add(new AssertTraceListener());
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
