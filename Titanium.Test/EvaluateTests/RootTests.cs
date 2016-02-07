using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class RootTests
	{
		[TestMethod]
		public void SquareRootOfIntegerSquare()
		{
			Common.EvaluateAndAssert("√4", "2");
		}

		[TestMethod]
		public void SquareRootOfIntegerNonSquare()
		{
			Common.EvaluateAndAssert("√2", "√(2)");
		}

		[TestMethod]
		public void SquareRootOfZero()
		{
			Common.EvaluateAndAssert("√0", "0");
		}

		[TestMethod]
		public void SquareRootOfFloatSquare()
		{
			Common.EvaluateAndAssert("√4.", "2.");
		}

		[TestMethod]
		public void SquareRootOfFloatNonSquare()
		{
			Common.EvaluateAndAssert("√2.", "1.4142135623731");
		}

		[TestMethod]
		public void SquareRootOfNegativeIntegerSquare()
		{
			Common.AssertThrows<NonRealResultException>("√(⁻4)");
		}

		[TestMethod]
		public void SquareRootOfNegativeIntegerNonSquare()
		{
			Common.AssertThrows<NonRealResultException>("√(⁻2)");
		}

		[TestMethod]
		public void SquareRootOfNegativeFloatSquare()
		{
			Common.AssertThrows<NonRealResultException>("√(⁻4.0)");
		}

		[TestMethod]
		public void SquareRootOfNegativeFloatNonSquare()
		{
			Common.AssertThrows<NonRealResultException>("√(⁻2.0)");
		}

		[TestMethod]
		public void SquareRootOfIntegerNonSquareWithParentheses()
		{
			Common.EvaluateAndAssert("√(2)", "√(2)");
		}

		[TestMethod]
		public void SquareRootAlternativeName()
		{
			Common.EvaluateAndAssert("4/sqrt(2)", "4/√(2)");
		}
	}
}
