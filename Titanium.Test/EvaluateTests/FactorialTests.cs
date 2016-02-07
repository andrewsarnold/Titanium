using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class FactorialTests
	{
		[TestMethod]
		public void FactorialOfZeroIsOne()
		{
			Common.EvaluateAndAssert("0!", "1");
		}

		[TestMethod]
		public void FactorialOfOneIsOne()
		{
			Common.EvaluateAndAssert("1!", "1");
		}

		[TestMethod]
		public void FactorialOfIntegerIsEvaluated()
		{
			Common.EvaluateAndAssert("4!", "24");
		}

		[TestMethod]
		public void NegativeFactorialOfIntegerIsEvaluated()
		{
			Common.EvaluateAndAssert("⁻4!", "⁻24");
		}

		[TestMethod]
		public void FactorialOfNegativeIntegerIsUndefined()
		{
			Common.EvaluateAndAssert("(⁻4)!", "undef");
		}

		[TestMethod]
		public void FactorialOfZeroFloatIsEvaluated()
		{
			Common.EvaluateAndAssert("0.0!", "1.");
		}

		[TestMethod]
		public void FactorialOfOneFloatIsEvaluated()
		{
			Common.EvaluateAndAssert("1.0!", "1.");
		}

		[TestMethod]
		public void FactorialOfFloatWithNoDecimalPortionIsEvaluated()
		{
			Common.EvaluateAndAssert("4.0!", "24.");
		}

		[TestMethod]
		public void FactorialOfFloatWithDecimalPortionIsNotEvaluated()
		{
			Common.EvaluateAndAssert("2.5!", "(2.5)!");
		}

		[TestMethod]
		public void NegativeFactorialOfFloatWithNoDecimalPortionIsEvaluated()
		{
			Common.EvaluateAndAssert("⁻4.0!", "⁻24.");
		}

		[TestMethod]
		public void FactorialOfNegativeFloatWithDecimalPortionIsNotEvaluated()
		{
			Common.EvaluateAndAssert("(⁻4.5)!", "(⁻4.5)!");
		}
	}
}