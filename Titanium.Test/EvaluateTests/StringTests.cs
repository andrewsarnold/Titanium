using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class StringTests
	{
		[TestMethod]
		public void StringIsItself()
		{
			Common.EvaluateAndAssert("\"string\"", "\"string\"");
		}

		[TestMethod]
		public void StringTimesIntegerIsUnchanged()
		{
			Common.EvaluateAndAssert("\"string\"*3", "3*\"string\"");
		}

		[TestMethod]
		public void StringTimesVariableIsUnchanged()
		{
			Common.EvaluateAndAssert("\"string\"*x", "x*\"string\"");
		}
	}
}
