using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class LogarithmTests
	{
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
