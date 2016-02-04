using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ListTests
	{
		[TestMethod]
		public void ListEvaluationTest()
		{
			Common.EvaluateAndAssert("{2/1}", "{2}");
			Common.EvaluateAndAssert("{2*2}", "{4}");
		}
	}
}
