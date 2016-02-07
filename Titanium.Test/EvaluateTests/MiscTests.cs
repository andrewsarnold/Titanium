using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class MiscTests
	{
		[TestMethod]
		public void EvaluateAlphaTest()
		{
			Common.EvaluateAndAssert("(12 - a)", "12-a");
			Common.EvaluateAndAssert("a * b -3", "a*b-3");
		}
	}
}
