using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class LogarithmTests
	{
		[TestMethod]
		public void LogTenOfTenIsOne()
		{
			Common.EvaluateAndAssert("log(10)", "1");
		}

		[TestMethod]
		public void LogTenOfOneHundredIsTwo()
		{
			Common.EvaluateAndAssert("log(100)", "2");
		}

		[TestMethod]
		public void LogTenOfIntegerReducesByLn()
		{
			Common.EvaluateAndAssert("log(3)", "ln(3)/ln(10)");
		}

		[TestMethod]
		public void LogTenOfNegativeIntegerIsNonReal()
		{
			Common.AssertThrows<NonRealResultException>("log(⁻1)");
		}

		[TestMethod]
		public void LogTenOfIntegerFractionExpands()
		{
			Common.EvaluateAndAssert("log(2/5)", "ln(2/5)/ln(10)");
		}

		[TestMethod]
		public void LogTenDivisionExpands()
		{
			Common.EvaluateAndAssert("log(6)/log(4)", "ln(6)/(2*ln(2))");
		}

		[TestMethod]
		public void NaturalLogOverLogTen()
		{
			Common.EvaluateAndAssert("ln(6)/log(4)", "ln(6)*ln(10)/(2*ln(2))");
		}

		[TestMethod]
		public void NaturalLogMinusLogTen()
		{
			Common.EvaluateAndAssert("ln(6)-log(4)", "ln(6)-2*ln(2)/ln(10)");
		}
	}
}
