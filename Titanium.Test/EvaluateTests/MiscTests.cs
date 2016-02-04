using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class MiscTests
	{
		[TestMethod]
		public void EvaluateNestedParenthesesTest()
		{
			Common.EvaluateAndAssert("(1)", "1");
			Common.EvaluateAndAssert("((1))", "1");
			Common.EvaluateAndAssert("(((1)))", "1");
			Common.EvaluateAndAssert("((((1))))", "1");
		}

		[TestMethod]
		public void EvaluateAlphaTest()
		{
			Common.EvaluateAndAssert("(12 - a)", "12-a");
			Common.EvaluateAndAssert("a * b -3", "a*b-3");
		}

		[TestMethod]
		public void EvaluateConstantTest()
		{
			Common.EvaluateAndAssert("π", Math.PI.ToString(CultureInfo.InvariantCulture));
			Common.EvaluateAndAssert("e", Math.E.ToString(CultureInfo.InvariantCulture));
		}

		[TestMethod]
		public void EvaluateErrorTest()
		{
			Common.AssertThrows<SyntaxErrorException>("12*");
			Common.AssertThrows<SyntaxErrorException>(".5.");
		}

		[TestMethod]
		public void EvaluateFactorialTest()
		{
			Common.EvaluateAndAssert("⁻10!", "1");
			Common.EvaluateAndAssert("0!", "1");
			Common.EvaluateAndAssert("1!", "1");
			Common.EvaluateAndAssert("3!", "6");
			Common.EvaluateAndAssert("4!", "24");
			Common.EvaluateAndAssert("2.5!", "(2.5)!");
		}
	}
}
