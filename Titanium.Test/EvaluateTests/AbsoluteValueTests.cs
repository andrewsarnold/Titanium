using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class AbsoluteValueTests
	{
		[TestMethod]
		public void AbsOfPositiveInteger()
		{
			Common.EvaluateAndAssert("abs(3)", "3");
		}

		[TestMethod]
		public void AbsOfNegativeInteger()
		{
			Common.EvaluateAndAssert("abs(⁻3)", "3");
		}

		[TestMethod]
		public void AbsOfZeroInteger()
		{
			Common.EvaluateAndAssert("abs(0)", "0");
		}

		[TestMethod]
		public void AbsOfPositiveFloat()
		{
			Common.EvaluateAndAssert("abs(3.4)", "3.4");
		}

		[TestMethod]
		public void AbsOfNegativeFloat()
		{
			Common.EvaluateAndAssert("abs(⁻3.4)", "3.4");
		}

		[TestMethod]
		public void AbsOfZeroFloat()
		{
			Common.EvaluateAndAssert("abs(0.0)", "0.");
		}

		[TestMethod]
		public void AbsOfConstant()
		{
			Common.EvaluateAndAssert("abs(e)", "e");
		}

		[TestMethod]
		public void AbsOfPositiveIntegerFraction()
		{
			Common.EvaluateAndAssert("abs(4/3)", "4/3");
		}

		[TestMethod]
		public void AbsOfNegativeIntegerFraction()
		{
			Common.EvaluateAndAssert("abs(⁻4/3)", "4/3");
		}

		[TestMethod]
		public void AbsOfZeroEvaluatableFunction()
		{
			Common.EvaluateAndAssert("abs(sin(0))", "0");
		}

		[TestMethod]
		public void AbsOfPositiveEvaluatableFunction()
		{
			Common.EvaluateAndAssert("abs(cos(0))", "1");
		}

		[TestMethod]
		public void AbsOfNegativeEvaluatableFunction()
		{
			Common.EvaluateAndAssert("abs(cos(π))", "1");
		}
		
		[TestMethod]
		public void AbsoluteValueOfPositiveInfinity()
		{
			Common.EvaluateAndAssert("abs(∞)", "∞");
		}

		[TestMethod]
		public void AbsoluteValueOfNegativeInfinity()
		{
			Common.EvaluateAndAssert("abs(⁻∞)", "∞");
		}
	}
}
