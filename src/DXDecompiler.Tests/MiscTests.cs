using NUnit.Framework;
using DXDecompiler.Util;

namespace DXDecompiler.Tests
{
	[TestFixture]
	class MiscTests
	{
		[Test]
		public void DecoderTest()
		{
			uint val = 0;
			val = 276824065;
			Assert.AreEqual(1, val.DecodeValue(0, 19), "Decode [0:19]");
			Assert.AreEqual(8, val.DecodeValue(20, 27), "Decode [20:27]");
			Assert.AreEqual(1, val.DecodeValue(28, 31), "Decode [28:31]");
			val = 542113796;
			Assert.AreEqual(4, val.DecodeValue(0, 19), "Decode [0:19]");
			Assert.AreEqual(5, val.DecodeValue(20, 27), "Decode [20:27]");
			Assert.AreEqual(2, val.DecodeValue(28, 31), "Decode [28:31]");
		}
	}
}
