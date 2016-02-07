using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ImpliedMultiplicationTests
	{
		[TestMethod]
		public void OperatorParenthesesOperator()
		{
			Common.EvaluateAndAssert("2(3)", "6");
		}

		[TestMethod]
		public void ParenthesesOperatorOperator()
		{
			Common.EvaluateAndAssert("(2)3", "6");
		}

		[TestMethod]
		public void ParenthesesOperatorParenthesesOperator()
		{
			Common.EvaluateAndAssert("(3)(2)", "6");
		}

		[TestMethod]
		public void OperatorFunction()
		{
			Common.EvaluateAndAssert("2cos(1)", "2*cos(1)");
		}

		[TestMethod]
		public void OperatorEvaluatableFunction()
		{
			Common.EvaluateAndAssert("2cos(0)", "2");
		}
	}
}
