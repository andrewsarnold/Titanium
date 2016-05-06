using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class NegationTests
	{
		[TestMethod]
		public void NegativeIntegerIsItself()
		{
			Common.EvaluateAndAssert("⁻1", "⁻1");
		}

		[TestMethod]
		public void NegativeAlphabeticFactorIsItself()
		{
			Common.EvaluateAndAssert("⁻a", "⁻a");
		}

		[TestMethod]
		public void NegationOfConstantIsNotExpanded()
		{
			Common.EvaluateAndAssert("⁻e", "⁻e");
		}

		[TestMethod]
		public void NegativeFunctionIsNotExpanded()
		{
			Common.EvaluateAndAssert("⁻sin(1)", "⁻sin(1)");
		}

		[TestMethod]
		public void NegationOfEvaluatableExpressionWithIntegers()
		{
			Common.EvaluateAndAssert("⁻(1+1)", "⁻2");
		}

		[TestMethod]
		public void NegationOfEvaluatableExpressionWithIntegerFractions()
		{
			Common.EvaluateAndAssert("⁻(1/2+1/2)", "⁻1");
		}

		[TestMethod]
		public void NegationOfEvaluatableExpressionOfFraction()
		{
			Common.EvaluateAndAssert("⁻(1/2+4/2)", "⁻5/2");
		}

		[TestMethod]
		public void NegationOfEvaluatableExpressionWithNegativeIntegerFractions()
		{
			Common.EvaluateAndAssert("⁻(1/2+⁻4/2)", "3/2");
		}

		[TestMethod]
		public void IntegerFractionWithNegativeNumeratorAndNegativeDenominator()
		{
			Common.EvaluateAndAssert("⁻5/⁻3", "5/3");
		}

		[TestMethod]
		public void NegativeExpressionOfNegativeInteger()
		{
			Common.EvaluateAndAssert("⁻(⁻3)", "3");
		}

		[TestMethod]
		public void NegativeExpressionOfNegativeExpressionOfNegativeInteger()
		{
			Common.EvaluateAndAssert("⁻(⁻(⁻3))", "⁻3");
		}

		[TestMethod]
		public void SimpleNegativeWithHyphen()
		{
			Common.EvaluateAndAssert("-1", "⁻1");
		}

		[TestMethod]
		public void SubtractNegativeWithHyphen()
		{
			Common.EvaluateAndAssert("3--1", "4");
		}

		[TestMethod]
		public void NegativeAlphabeticFactorWithHyphen()
		{
			Common.EvaluateAndAssert("-a", "⁻a");
		}

		[TestMethod]
		public void NegativeConstantWithHyphen()
		{
			Common.EvaluateAndAssert("-e", "⁻e");
		}

		[TestMethod]
		public void NegativeFunctionWithHyphen()
		{
			Common.EvaluateAndAssert("-sin(1)", "⁻sin(1)");
		}

		[TestMethod]
		public void NegationOfEvaluatableExpressionWithIntegersWithHyphen()
		{
			Common.EvaluateAndAssert("-(1+1)", "⁻2");
		}
	}
}
