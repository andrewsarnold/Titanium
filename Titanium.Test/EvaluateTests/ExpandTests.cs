using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ExpandTests
	{
		[TestMethod]
		public void ExpandFactorTest()
		{
			Common.EvaluateAndAssert("expand(x)", "x");
		}

		[TestMethod]
		public void ExpandFactorSquaredTest()
		{
			Common.EvaluateAndAssert("expand(x^2)", "x^2");
		}

		[TestMethod]
		public void ExpandToZeroTest()
		{
			Common.EvaluateAndAssert("expand(x^0)", "1");
		}

		[TestMethod]
		public void ExpandSquareOfSubtractionTest()
		{
			Common.EvaluateAndAssert("expand((x-5)^2)", "x^2-10*x+25");
		}

		[TestMethod]
		public void ExpandCubeOfSubtractionTest()
		{
			Common.EvaluateAndAssert("expand((x-5)^3)", "x^3-15*x^2+75*x-125");
		}

		[TestMethod]
		public void ExpandToNegativeTest()
		{
			Common.EvaluateAndAssert("expand((x-5)^⁻2)", "1/(x-5)^2");
		}
	}
}
