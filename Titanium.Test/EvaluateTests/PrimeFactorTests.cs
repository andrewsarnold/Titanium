using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class PrimeFactorTests
	{
		[TestMethod]
		public void PrimeFactorOfOneIsOne()
		{
			Common.EvaluateAndAssert("factor(1)", "1");
		}

		[TestMethod]
		public void PrimeFactorOfZeroIsZero()
		{
			Common.EvaluateAndAssert("factor(0)", "0");
		}

		[TestMethod]
		public void PrimeFactorOfPrimeIsPrime()
		{
			//Common.EvaluateAndAssert("factor(2)", "2");
			//Common.EvaluateAndAssert("factor(3)", "3");
			//Common.EvaluateAndAssert("factor(5)", "5");
			Common.EvaluateAndAssert("factor(23)", "23");
		}

		[TestMethod]
		public void PrimeFactorOfPowerOfTwoIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(128)", "2^7");
		}

		[TestMethod]
		public void PrimeFactorOfDivisibleNumberIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(12)", "2^2*3");
			Common.EvaluateAndAssert("factor(35)", "5*7");
			Common.EvaluateAndAssert("factor(100)", "2^2*5^2");
		}

		[TestMethod]
		public void PrimeFactorOfFloatDivisibleNumberIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(12.)", "(2.)^2*3.");
			Common.EvaluateAndAssert("factor(35.)", "5.*7.");
			Common.EvaluateAndAssert("factor(100.)", "(2.)^2*(5.)^2");
		}

		[TestMethod]
		public void PrimeFactorOfFloatPowerOfTwoIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(128.)", "(2.)^7");
		}

		[TestMethod]
		public void PrimeFactorOfFloatOneIsIntegerOne()
		{
			Common.EvaluateAndAssert("factor(1.0)", "1");
		}

		[TestMethod]
		public void PrimeFactorOfFloatZeroIsFloatZero()
		{
			Common.EvaluateAndAssert("factor(0.0)", "0.");
		}

		[TestMethod]
		public void PrimeFactorOfFloatPrimeIsFloatPrime()
		{
			Common.EvaluateAndAssert("factor(2.0)", "2.");
			Common.EvaluateAndAssert("factor(3.0)", "3.");
			Common.EvaluateAndAssert("factor(5.0)", "5.");
			Common.EvaluateAndAssert("factor(23.)", "23.");
		}

		[TestMethod]
		public void PrimeFactorOfFloatIsCalculatedWithDivision()
		{
			Common.EvaluateAndAssert("factor(0.2)", "1/(5.)");
			Common.EvaluateAndAssert("factor(8.6)", "43./(5.)");
		}

		[TestMethod]
		public void PrimeFactorOfAlphanumericIsItself()
		{
			Common.EvaluateAndAssert("factor(x)", "x");
		}

		[TestMethod]
		public void PrimeFactorOfAlphanumericExpressionIsItself()
		{
			Common.EvaluateAndAssert("factor(x*y)", "x*y");
		}
		
		[TestMethod]
		public void PrimeFactorOfNegativeOneIsNegativeOne()
		{
			Common.EvaluateAndAssert("factor(⁻1)", "⁻1");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeZeroIsZero()
		{
			Common.EvaluateAndAssert("factor(⁻0)", "0");
		}

		[TestMethod]
		public void PrimeFactorOfNegativePrimeIsNegativeOneTimesPrime()
		{
			Common.EvaluateAndAssert("factor(⁻2)", "⁻1*2");
			Common.EvaluateAndAssert("factor(⁻3)", "⁻1*3");
			Common.EvaluateAndAssert("factor(⁻5)", "⁻1*5");
		}

		[TestMethod]
		public void PrimeFactorOfNegativePowerOfTwoIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(⁻128)", "⁻1*2^7");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeDivisibleNumberIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(12)", "⁻1*2^2*3");
			Common.EvaluateAndAssert("factor(35)", "⁻1*5*7");
			Common.EvaluateAndAssert("factor(100)", "⁻1*2^2*5^2");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeFloatDivisibleNumberIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(⁻12.)", "⁻1.*(2.)^2*3.");
			Common.EvaluateAndAssert("factor(⁻35.)", "⁻1.*5.*7.");
			Common.EvaluateAndAssert("factor(⁻100.)", "⁻1.*(2.)^2*(5.)^2");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeFloatPowerOfTwoIsEvaluated()
		{
			Common.EvaluateAndAssert("factor(⁻128.)", "⁻1.*(2.)^7");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeFloatOneIsFloatNegativeOne()
		{
			Common.EvaluateAndAssert("factor(⁻1.0)", "⁻1.");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeFloatZeroIsFloatZero()
		{
			Common.EvaluateAndAssert("factor(⁻0.0)", "0.");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeFloatPrimeIsFloatPrime()
		{
			Common.EvaluateAndAssert("factor(⁻2.0)", "⁻1.*2.");
			Common.EvaluateAndAssert("factor(⁻3.0)", "⁻1.*3.");
			Common.EvaluateAndAssert("factor(⁻5.0)", "⁻1.*5.");
		}

		[TestMethod]
		public void PrimeFactorOfNegativeFloatIsCalculatedWithDivision()
		{
			Common.EvaluateAndAssert("factor(⁻0.2)", "⁻1./(5.)");
			Common.EvaluateAndAssert("factor(⁻8.6)", "⁻1.*43./(5.)");
		}
	}
}
