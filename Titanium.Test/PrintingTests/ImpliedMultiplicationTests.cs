using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.PrintingTests
{
	[TestClass]
	public class ImpliedMultiplicationTests
	{
		[TestMethod]
		public void AssertParenthesesOnFirstFactorImplyMultiplication()
		{
			Common.AssertPrinting("(a)b", "a*b");
		}

		[TestMethod]
		public void AssertParenthesesOnSecondFactorImplyMultiplication()
		{
			Common.AssertPrinting("a(b)", "a*b");
		}
		
		[TestMethod]
		public void AssertTwoSingleFactorComponentsAreMultiplied()
		{
			Common.AssertPrinting("(a)(b)", "a*b");
		}

		[TestMethod]
		public void AssertTwoDualComponentExpressionsAreMultiplied()
		{
			Common.AssertPrinting("(a+b)(c+d)", "(a+b)*(c+d)");
		}

		[TestMethod]
		public void AssertOperatorAndFunctionAreMultiplied()
		{
			Common.AssertPrinting("2cos(1)", "2*cos(1)");
		}
	}
}
