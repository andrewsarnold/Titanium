using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.PrintingTests
{
	[TestClass]
	public class ExponentTests
	{
		[TestMethod]
		public void AssertSimpleOperandExponentiationPrintsCorrectly()
		{
			Common.AssertPrinting("a^d", "a^d");
		}

		[TestMethod]
		public void AssertRedundantParentheseAreRemovedOnFirstOperand()
		{
			Common.AssertPrinting("(a)^b", "a^b");
		}

		[TestMethod]
		public void AssertRedundantParentheseAreRemovedOnSecondOperand()
		{
			Common.AssertPrinting("a^(b)", "a^b");
		}

		[TestMethod]
		public void AssertRedundantParentheseAreRemovedOnBothOperands()
		{
			Common.AssertPrinting("(a)^(b)", "a^b");
		}

		[TestMethod]
		public void AssertExpressionRetainsParenthesesOnFirstOperand()
		{
			Common.AssertPrinting("(a+b)^c+d", "(a+b)^c+d");
		}

		[TestMethod]
		public void AssertExpressionRetainsParenthesesOnSecondOperand()
		{
			Common.AssertPrinting("a+b^(c+d)", "a+b^(c+d)");
		}

		[TestMethod]
		public void AssertExpressionRetainsParentheseOnBothOperands()
		{
			Common.AssertPrinting("(a+b)^(c+d)", "(a+b)^(c+d)");
		}
	}
}
