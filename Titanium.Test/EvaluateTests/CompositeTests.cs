using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class CompositeTests
	{
		[TestMethod]
		public void DivisionOfIntegerByEvaluatableFunction()
		{
			Common.EvaluateAndAssert("2/cos(0)", "2");
		}

		[TestMethod]
		public void CompositeEvaluatableFactorials()
		{
			Common.EvaluateAndAssert("3!!", "720");
		}

		[TestMethod]
		public void CompositeEvaluatableExponentAndFactorial()
		{
			Common.EvaluateAndAssert("2^3!", "64");
		}

		[TestMethod]
		public void CompositeEvaluatableFactorialAndExponent()
		{
			Common.EvaluateAndAssert("2!^3", "8");
		}

		[TestMethod]
		public void CompositeEvaluatableFunction()
		{
			Common.EvaluateAndAssert("cos(sin(0))", "1");
		}

		[TestMethod]
		public void DoubleCompositeEvaluatableFunction()
		{
			Common.EvaluateAndAssert("cos(sin(sin(0)))", "1");
		}

		[TestMethod]
		public void CompositeNonevaluatableFunction()
		{
			Common.EvaluateAndAssert("cos(sin(5))", "cos(sin(5))");
		}

		[TestMethod]
		public void CompositeFactorialAndNonevaluatableFunction()
		{
			Common.EvaluateAndAssert("cos(5!)", "cos(120)");
		}
	}
}
