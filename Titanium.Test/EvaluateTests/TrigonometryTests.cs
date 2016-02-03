using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class TrigonometryTests
	{
		[TestMethod]
		public void EvaluateSinTest()
		{
			Common.EvaluateAndAssert("sin(1.3)", "0.963558185417193");
			Common.EvaluateAndAssert("sin(⁻1.3)", "⁻0.963558185417193");
			Common.EvaluateAndAssert("sin(0)", "0");
			Common.EvaluateAndAssert("sin(3)", "sin(3)");
			Common.EvaluateAndAssert("sin(π)", "0");
			Common.EvaluateAndAssert("sin(e)", "sin(e)");
		}

		[TestMethod]
		public void EvaluateCosTest()
		{
			Common.EvaluateAndAssert("cos(1.3)", "0.267498828624587");
			Common.EvaluateAndAssert("cos(⁻1.3)", "0.267498828624587");
			Common.EvaluateAndAssert("cos(0)", "1");
			Common.EvaluateAndAssert("cos(3)", "cos(3)");
			Common.EvaluateAndAssert("cos(π)", "⁻1");
		}

		[TestMethod]
		public void EvaluateTanTest()
		{
			Common.EvaluateAndAssert("tan(1.3)", "3.60210244796798");
			Common.EvaluateAndAssert("tan(⁻1.3)", "⁻3.60210244796798");
			Common.EvaluateAndAssert("tan(0)", "0");
			// todo: tan pi / 2
		}
	}
}
