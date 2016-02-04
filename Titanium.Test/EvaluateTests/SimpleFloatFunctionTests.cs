using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class SimpleFloatFunctionTests
	{
		[TestMethod]
		public void SimpleFloatFunctionTest()
		{
			Common.EvaluateAndAssert("ceil(1.3)", "2");
			Common.EvaluateAndAssert("floor(⁻1.3)", "⁻2");
		}
	}
}
