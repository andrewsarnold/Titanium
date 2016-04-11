using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class SyntaxErrorTests
	{
		[TestMethod]
		public void AssertOperatorNeedsTwoOperands()
		{
			Common.AssertThrows<SyntaxErrorException>("12*");
		}

		[TestMethod]
		public void AssertPeriodAfterFloatThrowsError()
		{
			Common.AssertThrows<SyntaxErrorException>(".5.");
		}
	}
}
