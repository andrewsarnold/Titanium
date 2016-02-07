using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class UnaryEvaluationTests
	{
		[TestMethod]
		public void IntegerEvaluatesToItself()
		{
			Common.EvaluateAndAssert("12", "12");
		}

		[TestMethod]
		public void FloatEvaluatesToItself()
		{
			Common.EvaluateAndAssert(".5", "0.5");
		}

		[TestMethod]
		public void DoubleNegatesCancelOnInteger()
		{
			Common.EvaluateAndAssert("⁻⁻3", "3");
		}

		[TestMethod]
		public void DoubleNegatesCancelOnFloat()
		{
			Common.EvaluateAndAssert("⁻⁻3.5", "3.5");
		}
	}
}
