using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Expressions;

namespace Titanium.Test
{
	[TestClass]
	public class EvaluateTests
	{
		[TestMethod]
		public void EvaluateNumberTest()
		{
			EvaluateAndAssert("12", "12");
			EvaluateAndAssert(".5", "0.5");
			EvaluateAndAssert("⁻⁻3", "3");
		}

		[TestMethod]
		public void EvaluateAdditionTest()
		{
			EvaluateAndAssert("12 + 3", "15");
			EvaluateAndAssert("12.5 + 3", "15.5");
			EvaluateAndAssert("12.5 + ⁻3", "9.5");
			EvaluateAndAssert("⁻1 + ⁻3", "⁻4");
		}

		[TestMethod]
		public void EvaluateSubtractionTest()
		{
			EvaluateAndAssert("12 - 3", "9");
			EvaluateAndAssert("12.5 - 3", "9.5");
			EvaluateAndAssert("12.5 - ⁻3", "15.5");
			EvaluateAndAssert("⁻1 - ⁻3", "2");
		}

		[TestMethod]
		public void EvaluateMultiplicationTest()
		{
			EvaluateAndAssert("2 * 3", "6");
			EvaluateAndAssert("2 * 0.5", "1");
			EvaluateAndAssert("⁻2 * 0.5", "⁻1");
			EvaluateAndAssert("⁻2 * ⁻0.5", "1");
		}

		[TestMethod]
		public void EvaluateDivisionTest()
		{
			EvaluateAndAssert("6 / 2", "3");
			EvaluateAndAssert("0 / 1", "0");
			EvaluateAndAssert("1 / 1", "1");
			EvaluateAndAssert("2 / 4", "1/2");
			EvaluateAndAssert("3 / 9", "1/3");
			EvaluateAndAssert("10 / 5", "2");
			EvaluateAndAssert("15 / 10", "3/2");
			AssertThrows("1 / 0");
		}

		[TestMethod]
		public void EvaluateExponentTest()
		{
			EvaluateAndAssert("3 ^ 1", "3");
			EvaluateAndAssert("3 ^ 2", "9");
			EvaluateAndAssert("4 ^ .5", "2");
			EvaluateAndAssert("4 ^ (1/2.)", "2");
			EvaluateAndAssert("3 ^ 0", "1");
		}

		[TestMethod]
		public void EvaluateNestedParenthesesTest()
		{
			EvaluateAndAssert("(1)", "1");
			EvaluateAndAssert("((1))", "1");
			EvaluateAndAssert("(((1)))", "1");
			EvaluateAndAssert("((((1))))", "1");
		}

		[TestMethod]
		public void EvaluateComplexTest()
		{
			EvaluateAndAssert("(12 - 3) + 5 / 4", "41/4");
			EvaluateAndAssert("(12 - 3) + ((5 * 4)-1)-2", "26");

			EvaluateAndAssert("3/1", "3");
			EvaluateAndAssert("4+3/1", "7");
			EvaluateAndAssert("3/1+2", "5");
			EvaluateAndAssert("4+3/1+2", "9");
			EvaluateAndAssert("4+(3/1)+2", "9");
		}

		[TestMethod]
		public void EvaluateAlphaTest()
		{
			EvaluateAndAssert("(12 - a)", "12-a");
			EvaluateAndAssert("a * b -3", "a*b-3");
		}

		[TestMethod]
		public void EvaluateConstantTest()
		{
			EvaluateAndAssert("π", Math.PI.ToString(CultureInfo.InvariantCulture));
			EvaluateAndAssert("e", Math.E.ToString(CultureInfo.InvariantCulture));
		}

		[TestMethod]
		public void EvaluateErrorTest()
		{
			AssertThrows("aa4");
			AssertThrows("12*");
			AssertThrows(".5.");
		}

		[TestMethod]
		public void EvaluateFactorialTest()
		{
			EvaluateAndAssert("⁻10!", "1");
			EvaluateAndAssert("0!", "1");
			EvaluateAndAssert("1!", "1");
			EvaluateAndAssert("3!", "6");
			EvaluateAndAssert("4!", "24");
			EvaluateAndAssert("2.5!", "(2.5)!");
		}

		[TestMethod]
		public void EvaluateTrigTest()
		{
			EvaluateAndAssert("sin(1.3)", "0.963558185417193");
			EvaluateAndAssert("sin(⁻1.3)", "⁻0.963558185417193");
			EvaluateAndAssert("sin(0)", "0");
			EvaluateAndAssert("sin(3)", "sin(3)");
			EvaluateAndAssert("sin(π)", "0");
			EvaluateAndAssert("sin(e)", "sin(e)");

			EvaluateAndAssert("cos(1.3)", "0.267498828624587");
			EvaluateAndAssert("cos(⁻1.3)", "0.267498828624587");
			EvaluateAndAssert("cos(0)", "1");
			EvaluateAndAssert("cos(3)", "cos(3)");
			EvaluateAndAssert("cos(π)", "⁻1");

			EvaluateAndAssert("tan(1.3)", "0.267498828624587");
			EvaluateAndAssert("tan(⁻1.3)", "0.267498828624587");
			EvaluateAndAssert("tan(0)", "1");
			// todo: tan pi / 2
		}

		[TestMethod]
		public void EvaluateFractionTest()
		{
			EvaluateAndAssert("1/2", "1/2");
			EvaluateAndAssert("2/1", "2");
			EvaluateAndAssert("4+1/2", "9/2");
			EvaluateAndAssert("1/2+1/2", "1");
			EvaluateAndAssert("1/2*1/2", "1/4");
		}

		private static void EvaluateAndAssert(string input, string output)
		{
			var expression = Expression.ParseExpression(input);
			var result = expression.Evaluate();
			Assert.AreEqual(output, result.ToString());
		}

		private static void AssertThrows(string input)
		{
			try
			{
				EvaluateAndAssert(input, string.Empty);
				Assert.Fail();
			}
			catch (Exception)
			{
				Assert.IsTrue(true);
			}
		}
	}
}
