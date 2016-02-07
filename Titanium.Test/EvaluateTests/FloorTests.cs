using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class FloorTests
	{
		[TestMethod]
		public void FloorOfNegativeIntegerTest()
		{
			Common.EvaluateAndAssert("floor(⁻1.3)", "⁻2.");
		}

		[TestMethod]
		public void FloorOfNegativeFloatTest()
		{
			Common.EvaluateAndAssert("floor(⁻2)", "⁻2");
		}

		[TestMethod]
		public void FloorOfZeroFloatTest()
		{
			Common.EvaluateAndAssert("floor(0.0)", "0.");
		}

		[TestMethod]
		public void FloorOfZeroIntegerTest()
		{
			Common.EvaluateAndAssert("floor(0)", "0");
		}

		[TestMethod]
		public void FloorOfPositiveIntegerTest()
		{
			Common.EvaluateAndAssert("floor(2)", "2");
		}

		[TestMethod]
		public void FloorOfPositiveFloatTest()
		{
			Common.EvaluateAndAssert("floor(1.3)", "1.");
		}
	}
}
