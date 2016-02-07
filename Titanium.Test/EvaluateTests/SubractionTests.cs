using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class SubractionTests
	{
		[TestMethod]
		public void IntegerMinusInteger()
		{
			Common.EvaluateAndAssert("12 - 3", "9");
		}

		[TestMethod]
		public void FloatMinusInteger()
		{
			Common.EvaluateAndAssert("12.5 - 3", "9.5");
		}

		[TestMethod]
		public void IntegerMinusFloat()
		{
			Common.EvaluateAndAssert("12 - 3.5", "8.5");
		}

		[TestMethod]
		public void FloatMinusNegativeInteger()
		{
			Common.EvaluateAndAssert("12.5 - ⁻3", "15.5");
		}

		[TestMethod]
		public void NegativeIntegerMinusNegativeInteger()
		{
			Common.EvaluateAndAssert("⁻1 - ⁻3", "2");
		}
	}
}
