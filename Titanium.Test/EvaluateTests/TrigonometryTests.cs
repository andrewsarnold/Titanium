using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class TrigonometryTests
	{
		[TestMethod]
		public void SineOfFloatTest()
		{
			Common.EvaluateAndAssert("sin(1.3)", ".963558185417193");
		}

		[TestMethod]
		public void SineOfZeroTest()
		{
			Common.EvaluateAndAssert("sin(0)", "0");
		}

		[TestMethod]
		public void SineOfNonspecialIntegerTest()
		{
			Common.EvaluateAndAssert("sin(3)", "sin(3)");
		}

		[TestMethod]
		public void SineOfPiTest()
		{
			Common.EvaluateAndAssert("sin(π)", "0");
		}

		[TestMethod]
		public void SineOfPiOverTwoTest()
		{
			Common.EvaluateAndAssert("sin(π/2)", "1");
		}

		[TestMethod]
		public void SineOfAlphabeticOperantTest()
		{
			Common.EvaluateAndAssert("sin(a)", "sin(a)");
		}

		[TestMethod]
		public void CosineOfFloatTest()
		{
			Common.EvaluateAndAssert("cos(1.3)", ".267498828624587");
		}

		[TestMethod]
		public void CosineOfZeroTest()
		{
			Common.EvaluateAndAssert("cos(0)", "1");
		}

		[TestMethod]
		public void CosineOfNonspecialIntegerTest()
		{
			Common.EvaluateAndAssert("cos(3)", "cos(3)");
		}

		[TestMethod]
		public void CosineOfPiTest()
		{
			Common.EvaluateAndAssert("cos(π)", "⁻1");
		}

		[TestMethod]
		public void CosineOfPiOverTwoTest()
		{
			Common.EvaluateAndAssert("cos(π/2)", "0");
		}

		[TestMethod]
		public void CosineOfAlphabeticOperantTest()
		{
			Common.EvaluateAndAssert("cos(a)", "cos(a)");
		}

		[TestMethod]
		public void TangentOfFloatTest()
		{
			Common.EvaluateAndAssert("tan(1.3)", "3.60210244796798");
		}

		[TestMethod]
		public void TangentOfZeroTest()
		{
			Common.EvaluateAndAssert("tan(0)", "0");
		}

		[TestMethod]
		public void TangentOfNonspecialIntegerTest()
		{
			Common.EvaluateAndAssert("tan(3)", "tan(3)");
		}

		[TestMethod]
		public void TangentOfPiTest()
		{
			Common.EvaluateAndAssert("tan(π)", "0");
		}

		[TestMethod]
		public void TangentOfPiOverTwoTest()
		{
			Common.EvaluateAndAssert("tan(π/2)", "undef");
		}

		[TestMethod]
		public void TangentOfAlphabeticOperantTest()
		{
			Common.EvaluateAndAssert("tan(a)", "tan(a)");
		}

		// Todo: Special results like sin(pi/2) = sqrt(2)/2
	}
}
