using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ArithmeticTests
	{
		[TestMethod]
		public void EvaluateNumberTest()
		{
			Common.EvaluateAndAssert("12", "12");
			Common.EvaluateAndAssert(".5", "0.5");
			Common.EvaluateAndAssert("⁻⁻3", "3");
		}

		[TestMethod]
		public void EvaluateAdditionTest()
		{
			Common.EvaluateAndAssert("12 + 3", "15");
			Common.EvaluateAndAssert("12.5 + 3", "15.5");
			Common.EvaluateAndAssert("12.5 + ⁻3", "9.5");
			Common.EvaluateAndAssert("⁻1 + ⁻3", "⁻4");
		}

		[TestMethod]
		public void EvaluateSubtractionTest()
		{
			Common.EvaluateAndAssert("12 - 3", "9");
			Common.EvaluateAndAssert("12.5 - 3", "9.5");
			Common.EvaluateAndAssert("12.5 - ⁻3", "15.5");
			Common.EvaluateAndAssert("⁻1 - ⁻3", "2");
		}

		[TestMethod]
		public void EvaluateMultiplicationTest()
		{
			Common.EvaluateAndAssert("2 * 3", "6");
			Common.EvaluateAndAssert("2 * 0.5", "1");
			Common.EvaluateAndAssert("⁻2 * 0.5", "⁻1");
			Common.EvaluateAndAssert("⁻2 * ⁻0.5", "1");
		}

		[TestMethod]
		public void EvaluateDivisionTest()
		{
			Common.EvaluateAndAssert("6 / 2", "3");
			Common.EvaluateAndAssert("0 / 1", "0");
			Common.EvaluateAndAssert("1 / 1", "1");
			Common.EvaluateAndAssert("2 / 4", "1/2");
			Common.EvaluateAndAssert("3 / 9", "1/3");
			Common.EvaluateAndAssert("10 / 5", "2");
			Common.EvaluateAndAssert("15 / 10", "3/2");
			Common.AssertThrows("1 / 0");
		}

		[TestMethod]
		public void EvaluateExponentTest()
		{
			Common.EvaluateAndAssert("3 ^ 1", "3");
			Common.EvaluateAndAssert("3 ^ 2", "9");
			Common.EvaluateAndAssert("4 ^ .5", "2");
			Common.EvaluateAndAssert("4 ^ (1/2.)", "2");
			Common.EvaluateAndAssert("3 ^ 0", "1");
		}

		[TestMethod]
		public void EvaluateComplexTest()
		{
			Common.EvaluateAndAssert("(12 - 3) + 5 / 4", "41/4");
			Common.EvaluateAndAssert("(12 - 3) + ((5 * 4)-1)-2", "26");

			Common.EvaluateAndAssert("3/1", "3");
			Common.EvaluateAndAssert("4+3/1", "7");
			Common.EvaluateAndAssert("3/1+2", "5");
			Common.EvaluateAndAssert("4+3/1+2", "9");
			Common.EvaluateAndAssert("4+(3/1)+2", "9");
		}

		[TestMethod]
		public void EvaluateFractionTest()
		{
			Common.EvaluateAndAssert("1/2", "1/2");
			Common.EvaluateAndAssert("2/1", "2");
			Common.EvaluateAndAssert("4+1/2", "9/2");
			Common.EvaluateAndAssert("1/2+1/2", "1");
			Common.EvaluateAndAssert("1/2*1/2", "1/4");
		}

		[TestMethod]
		public void EvaluateImpliedMultiplicationTest()
		{
			Common.EvaluateAndAssert("2(3)", "6");
			Common.EvaluateAndAssert("(2)3", "6");
			Common.EvaluateAndAssert("(3)(2)", "6");
			Common.EvaluateAndAssert("2cos(1)", "2*cos(1)");
			Common.EvaluateAndAssert("2cos(0)", "2");
		}
	}
}
