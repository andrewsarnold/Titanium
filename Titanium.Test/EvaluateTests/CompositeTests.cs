using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class CompositeTests
	{
		[TestMethod]
		public void MultiFunctionTest()
		{
			Common.EvaluateAndAssert("3!!", "720");
			Common.EvaluateAndAssert("cos(sin(0))", "1");
			Common.EvaluateAndAssert("cos(sin(sin(0)))", "1");
			Common.EvaluateAndAssert("cos(sin(5))", "cos(sin(5))");
			Common.EvaluateAndAssert("cos(5!)", "cos(120)");
		}
	}
}
