using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class LogarithmTests
	{
		[TestMethod]
		public void NaturalLogTest()
		{
			Common.EvaluateAndAssert("ln(1)", "0");
			Common.EvaluateAndAssert("ln(e)", "1");
			Common.EvaluateAndAssert("ln(e^2)", "2");
			Common.EvaluateAndAssert("ln(e^⁻5)", "⁻5");
			Common.EvaluateAndAssert("ln(0)", "⁻∞");
			Common.AssertThrows<NonRealResultException>("ln(⁻1)");
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

		[TestMethod]
		public void LogTenTest()
		{
			Common.EvaluateAndAssert("log(10)", "1");
			Common.EvaluateAndAssert("log(100)", "2");
			Common.EvaluateAndAssert("log(3)", "ln(3)/ln(10)");
			Common.AssertThrows<NonRealResultException>("log(⁻1)");
			Common.EvaluateAndAssert("log(2/5)", "⁻ln(5/2)/(ln(10))");
			Common.EvaluateAndAssert("log(6)/log(4)", "ln(6)/(2*ln(2))");
		}

		[TestMethod]
		public void MixedLogTest()
		{
			Common.EvaluateAndAssert("ln(6)/log(4)", "ln(10)*ln(6)/(2*ln(2))");
			Common.EvaluateAndAssert("ln(6)-log(4)", "ln(6)-2*ln(2)/(ln(10))");
		}
	}
}
