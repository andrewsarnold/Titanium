using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class IntegerFractionTests
	{
		[TestMethod]
		public void FractionIsItself()
		{
			Common.EvaluateAndAssert("1/2", "1/2");
		}

		[TestMethod]
		public void FractionWithOneDenominator()
		{
			Common.EvaluateAndAssert("2/1", "2");
		}

		[TestMethod]
		public void IntegerPlusFraction()
		{
			Common.EvaluateAndAssert("4+1/2", "9/2");
		}

		[TestMethod]
		public void FractionPlusFraction()
		{
			Common.EvaluateAndAssert("1/2+1/2", "1");
		}

		[TestMethod]
		public void FractionMinusFraction()
		{
			Common.EvaluateAndAssert("5/2-2/2", "3/2");
		}

		[TestMethod]
		public void FractionTimesFraction()
		{
			Common.EvaluateAndAssert("1/2*1/2", "1/4");
		}

		[TestMethod]
		public void FractionDividedByFraction()
		{
			Common.EvaluateAndAssert("(1/2)/(4/5)", "5/8");
		}
	}
}
