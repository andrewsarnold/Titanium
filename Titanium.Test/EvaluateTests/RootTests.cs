using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class RootTests
	{
		[TestMethod]
		public void SquareRootTests()
		{
			Common.EvaluateAndAssert("√4", "2");
			Common.EvaluateAndAssert("√4", "2");
			Common.EvaluateAndAssert("√2", "√(2)");
			Common.EvaluateAndAssert("√(2)", "√(2)");
			Common.EvaluateAndAssert("4/√(2)", "4/√(2)");
			Common.EvaluateAndAssert("4/sqrt(2)", "4/√(2)");
		}
	}
}
