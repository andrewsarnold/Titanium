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
		public void NaturalLogOfZero()
		{
			Common.EvaluateAndAssert("ln(0)", "⁻∞");
		}

		[TestMethod]
		public void NaturalLogTest()
		{
			Common.EvaluateAndAssert("ln(1.3)", "0.262364");
			Common.EvaluateAndAssert("ln(1/2)", "⁻ln(2)");
			Common.EvaluateAndAssert("ln(√(2))", "ln(2)/2");
			Common.EvaluateAndAssert("ln(e/2)", "1-ln(2)");
			Common.EvaluateAndAssert("ln(e/5)", "1-ln(5)");
			Common.EvaluateAndAssert("ln(3e/5)", "ln(3/5)+1");
			Common.EvaluateAndAssert("ln(6)-ln(4)", "ln(3/2)");
			Common.EvaluateAndAssert("ln(6)+ln(4)", "ln(24)");
			Common.EvaluateAndAssert("ln(6)*ln(4)", "2*ln(6)*ln(2)");
			Common.EvaluateAndAssert("ln(6)/ln(4)", "ln(6)/(2*ln(2))");
			Common.EvaluateAndAssert("ln(6)^ln(4)", "ln(6)^(2*ln(2))");
		}
	}
}
