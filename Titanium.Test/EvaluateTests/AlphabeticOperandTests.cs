using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class AlphabeticOperandTests
	{
		[TestMethod]
		public void AssertAlphabeticComponentIsParsed()
		{
			Common.EvaluateAndAssert("(12 - a)", "12-a");
		}

		[TestMethod]
		public void AssertAlphabeticFactorIsParsed()
		{
			Common.EvaluateAndAssert("a * b -3", "a*b-3");
		}

		[TestMethod]
		public void AssertComponentsAreSorted()
		{
			Common.EvaluateAndAssert("x + 5", "x+5");
			Common.EvaluateAndAssert("5 + x", "x+5");
			Common.EvaluateAndAssert("x - 5", "x-5");
			Common.EvaluateAndAssert("5 - x", "5-x");
		}
	}
}
