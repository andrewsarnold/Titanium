using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class NestedParenthesesTests
	{
		[TestMethod]
		public void NestedParenthesesTest()
		{
			Common.EvaluateAndAssert("(1)", "1");
		}

		[TestMethod]
		public void DoubleNestedParenthesesTest()
		{
			Common.EvaluateAndAssert("((1))", "1");
		}

		[TestMethod]
		public void TripleNestedParenthesesTest()
		{
			Common.EvaluateAndAssert("(((1)))", "1");
		}

		[TestMethod]
		public void QuadrupleNestedParenthesesTest()
		{
			Common.EvaluateAndAssert("((((1))))", "1");
		}
	}
}
