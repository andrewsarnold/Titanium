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
	}
}
