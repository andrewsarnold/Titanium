using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class AdditionTests
	{
		[TestMethod]
		public void IntegerPlusInteger()
		{
			Common.EvaluateAndAssert("12 + 3", "15");
		}

		[TestMethod]
		public void FloatPlusInteger()
		{
			Common.EvaluateAndAssert("12.5 + 3", "15.5");
		}

		[TestMethod]
		public void IntegerPlusFloat()
		{
			Common.EvaluateAndAssert("3 + 12.5", "15.5");
		}

		[TestMethod]
		public void FloatPlusNegativeInteger()
		{
			Common.EvaluateAndAssert("12.5 + ⁻3", "9.5");
		}

		[TestMethod]
		public void NegativeIntegerPlusNegativeInteger()
		{
			Common.EvaluateAndAssert("⁻1 + ⁻3", "⁻4");
		}
	}
}
