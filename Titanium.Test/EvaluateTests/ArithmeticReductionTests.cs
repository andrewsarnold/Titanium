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
		public void AddedIdenticalComponents()
		{
			Common.EvaluateAndAssert("3x + 3x", "6*x");
		}

		[TestMethod]
		public void SubtractedIdenticalComponents()
		{
			Common.EvaluateAndAssert("3x - 3x", "0");
		}

		[TestMethod]
		public void AddedComponentsSharingAFactor()
		{
			Common.EvaluateAndAssert("3x + 4x", "7*x");
		}

		[TestMethod]
		public void MultipliedFactorsSharingAComponent()
		{
			Common.EvaluateAndAssert("(x-5)*(x-5)", "(x-5)^2");
		}

		[TestMethod]
		public void ExponentiatedFactorsSharingAComponent()
		{
			Common.EvaluateAndAssert("(x-5)^3*(x-5)", "(x-5)^4");
		}
	}
}
