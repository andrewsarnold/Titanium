using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class NaturalLogarithmTests
	{
		[TestMethod]
		public void NaturalLogOfOne()
		{
			Common.EvaluateAndAssert("ln(1)", "0");
		}

		[TestMethod]
		public void NaturalLogOfE()
		{
			Common.EvaluateAndAssert("ln(e)", "1");
		}

		[TestMethod]
		public void NaturalLogOfEToAPower()
		{
			Common.EvaluateAndAssert("ln(e^2)", "2");
		}

		[TestMethod]
		public void NaturalLogOfNegativeInteger()
		{
			Common.AssertThrows<NonRealResultException>("ln(⁻1)");
		}

		[TestMethod]
		public void NaturalLogOfNegativeFloat()
		{
			Common.AssertThrows<NonRealResultException>("ln(⁻1.3)");
		}

		[TestMethod]
		public void NaturalLogOfZero()
		{
			Common.EvaluateAndAssert("ln(0)", "⁻∞");
		}

		[TestMethod]
		public void NaturalLogOfPositiveFloat()
		{
			Common.EvaluateAndAssert("ln(1.3)", ".262364264467491");
		}

		[TestMethod]
		public void NaturalLogOfOneHalf()
		{
			Common.EvaluateAndAssert("ln(1/2)", "⁻ln(2)");
		}

		[TestMethod]
		public void NaturalLogOfIntegerRoot()
		{
			Common.EvaluateAndAssert("ln(√(5))", "ln(5)/2");
		}

		[TestMethod]
		public void NaturalLogOfEOverInteger()
		{
			Common.EvaluateAndAssert("ln(e/5)", "1-ln(5)");
		}

		[TestMethod]
		public void NaturalLogOfEInIntegerFraction()
		{
			Common.EvaluateAndAssert("ln(3e/5)", "ln(3/5)+1");
		}

		[TestMethod]
		public void NaturalLogsOfIntegerAdded()
		{
			Common.EvaluateAndAssert("ln(6)+ln(4)", "ln(24)");
		}

		[TestMethod]
		public void NaturalLogsOfIntegerSubtracted()
		{
			Common.EvaluateAndAssert("ln(6)-ln(4)", "ln(3/2)");
		}

		[TestMethod]
		public void NaturalLogsOfIntegerMultiplied()
		{
			Common.EvaluateAndAssert("ln(6)*ln(4)", "2*ln(6)*ln(2)");
		}

		[TestMethod]
		public void NaturalLogsOfIntegerDivided()
		{
			Common.EvaluateAndAssert("ln(6)/ln(4)", "ln(6)/(2*ln(2))");
		}

		[TestMethod]
		public void NaturalLogsOfIntegerRaised()
		{
			Common.EvaluateAndAssert("ln(6)^ln(4)", "ln(6)^(2*ln(2))");
		}
	}
}
