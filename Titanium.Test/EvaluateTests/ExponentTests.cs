using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ExponentTests
	{
		[TestMethod]
		public void IntegerRaisedToOne()
		{
			Common.EvaluateAndAssert("3 ^ 1", "3");
		}

		[TestMethod]
		public void IntegerRaisedToInteger()
		{
			Common.EvaluateAndAssert("3 ^ 2", "9");
		}

		[TestMethod]
		public void IntegerRaisedToFloat()
		{
			Common.EvaluateAndAssert("4 ^ .5", "2.");
		}

		[TestMethod]
		public void IntegerRaisedToFloatFraction()
		{
			Common.EvaluateAndAssert("4 ^ (1/2.)", "2");
		}
		
		[TestMethod]
		public void IntegerRaisedToZero()
		{
			Common.EvaluateAndAssert("3 ^ 0", "1");
		}

		[TestMethod]
		public void FloatRaisedToOne()
		{
			Common.EvaluateAndAssert("3.5 ^ 1", "3.5");
		}

		[TestMethod]
		public void FloatRaisedToFloat()
		{
			Common.EvaluateAndAssert("3.0 ^ 2.0", "9.");
		}

		[TestMethod]
		public void FloatRaisedToZero()
		{
			Common.EvaluateAndAssert("3.5 ^ 0", "1.");
		}
	}
}
