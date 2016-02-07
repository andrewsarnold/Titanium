using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ArithmeticTests
	{
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
			Common.EvaluateAndAssert("3*4/3", "4");
			Common.EvaluateAndAssert("3*(4/3)", "4");
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
