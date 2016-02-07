using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class DivisionTests
	{
		[TestMethod]
		public void IntegerDividedByInteger()
		{
			Common.EvaluateAndAssert("5 / 8", "5/8");
		}

		[TestMethod]
		public void LargerIntegerDividedBySmallerDivisibleInteger()
		{
			Common.EvaluateAndAssert("6 / 2", "3");
		}

		[TestMethod]
		public void ZeroDividedByInteger()
		{
			Common.EvaluateAndAssert("0 / 12", "0");
		}

		[TestMethod]
		public void IntegerDividedByItself()
		{
			Common.EvaluateAndAssert("1 / 1", "1");
		}

		[TestMethod]
		public void SmallerIntegerDividedByLargerDivisibleInteger()
		{
			Common.EvaluateAndAssert("2 / 4", "1/2");
		}

		[TestMethod]
		public void IntegerDivisionByZero()
		{
			Common.AssertThrows<DivideByZeroException>("1 / 0");
		}

		[TestMethod]
		public void IntegerDividedByFloat()
		{
			Common.EvaluateAndAssert("5 / 8.0", "0.625");
		}

		[TestMethod]
		public void FloatDividedByInteger()
		{
			Common.EvaluateAndAssert("5.0 / 8", "0.625");
		}
		
		[TestMethod]
		public void ZeroDividedByFloat()
		{
			Common.EvaluateAndAssert("0 / 12.4", "0.0");
		}

		[TestMethod]
		public void FloatDividedByItself()
		{
			Common.EvaluateAndAssert("1.4 / 1.4", "1.");
		}

		[TestMethod]
		public void FloatDivisionByZero()
		{
			Common.AssertThrows<DivideByZeroException>("1.0 / 0");
		}
	}
}
