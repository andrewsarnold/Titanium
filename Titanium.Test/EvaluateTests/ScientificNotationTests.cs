using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ScientificNotationTests
	{
		[TestMethod]
		public void SmallIntegerIsExpanded()
		{
			Common.EvaluateAndAssert("1E4", "10000.");
		}

		[TestMethod]
		public void SmallFloatIsExpanded()
		{
			Common.EvaluateAndAssert("1.E4", "10000.");
		}

		[TestMethod]
		public void LargeNumberIsNotExpanded()
		{
			Common.EvaluateAndAssert("1E8", "1.E8");
		}
	}
}
