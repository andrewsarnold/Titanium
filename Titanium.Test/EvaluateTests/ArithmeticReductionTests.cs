using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ArithmeticReductionTests
	{
		[TestMethod]
		public void VariableDividedByItselfIsOne()
		{
			Common.EvaluateAndAssert("x/x", "1");
		}

		[TestMethod]
		public void VariableTimesItselfIsSquared()
		{
			Common.EvaluateAndAssert("x*x", "x^2");
		}

		[TestMethod]
		public void AddedComponentsSharingAFactor()
		{
			Common.EvaluateAndAssert("3x + 4x", "7*x");
		}
	}
}
