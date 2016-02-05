using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class NegationTests
	{
		[TestMethod]
		public void SimpleNegationTest()
		{
			Common.EvaluateAndAssert("⁻e", "⁻e");
			Common.EvaluateAndAssert("⁻1", "⁻1");
			Common.EvaluateAndAssert("⁻a", "⁻a");
			Common.EvaluateAndAssert("⁻sin(1)", "⁻sin(1)");
		}

		[TestMethod]
		public void ComplexNegationTest()
		{
			Common.EvaluateAndAssert("⁻(1+1)", "⁻2");
			Common.EvaluateAndAssert("⁻(1/2+1/2)", "⁻1");
			Common.EvaluateAndAssert("⁻(1/2+4/2)", "⁻5/2");
			Common.EvaluateAndAssert("⁻(1/2+⁻4/2)", "3/2");
			Common.EvaluateAndAssert("⁻5/⁻3", "5/3");
			Common.EvaluateAndAssert("⁻(⁻3)", "3");
			Common.EvaluateAndAssert("⁻(⁻(⁻3))", "⁻3");
		}
	}
}
