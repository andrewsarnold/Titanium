using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Expressions;

namespace Titanium.Test
{
	[TestClass]
	public class PrintTests
	{
		[TestMethod]
		public void PrintImpliedMultiplicationTest()
		{
			AssertPrinting("ad", "ad");
			AssertPrinting("a(b)", "a*b");
			AssertPrinting("(a)b", "a*b");
			AssertPrinting("(a)(b)", "a*b");
			AssertPrinting("(a+b)(c+d)", "(a+b)*(c+d)");
		}

		[TestMethod]
		public void PrintExponentsTest()
		{
			AssertPrinting("a^d", "a^d");
			AssertPrinting("a^(b)", "a^b");
			AssertPrinting("(a)^b", "a^b");
			AssertPrinting("(a)^(b)", "a^b");
			AssertPrinting("(a+b)^(c+d)", "(a+b)^(c+d)");
			AssertPrinting("a+b^(c+d)", "a+b^(c+d)");
			AssertPrinting("(a+b)^c+d", "(a+b)^c+d");
		}

		private static void AssertPrinting(string input, string output)
		{
			Assert.AreEqual(output, Expression.ParseExpression(input).ToString());
		}
	}
}
