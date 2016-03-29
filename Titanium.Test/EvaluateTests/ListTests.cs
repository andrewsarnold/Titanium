using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ListTests
	{
		[TestMethod]
		public void EvaluateSingleElementList()
		{
			Common.EvaluateAndAssert("{2*2}", "{4}");
		}

		[TestMethod]
		public void ListTimesInteger()
		{
			Common.EvaluateAndAssert("{1,2}*3", "{3,6}");
		}

		[TestMethod]
		public void ListDividedByInteger()
		{
			Common.EvaluateAndAssert("{4,5}/2", "{2,5/2}");
		}

		[TestMethod]
		public void ListRaisedToInteger()
		{
			Common.EvaluateAndAssert("{4,5}^2", "{16,25}");
		}

		[TestMethod]
		public void ListTimesFloat()
		{
			Common.EvaluateAndAssert("{1,2}*3.", "{3.,6.}");
		}

		[TestMethod]
		public void ListDividedByFloat()
		{
			Common.EvaluateAndAssert("{4,5}/2.", "{2.,2.5}");
		}

		[TestMethod]
		public void ListRaisedToFloat()
		{
			Common.EvaluateAndAssert("{4,5}^2.", "{16.,25.}");
		}

		[TestMethod]
		public void ListTimesExpression()
		{
			Common.EvaluateAndAssert("{1,2}*(4.8-4.2)", "{.6,1.2}");
		}

		[TestMethod]
		public void EmptyListTimesOperand()
		{
			Common.EvaluateAndAssert("{}*3", "{}");
		}
	}
}
