using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class CeilingTests
	{
		[TestMethod]
		public void CeilingOfNegativeIntegerTest()
		{
			Common.EvaluateAndAssert("ceiling(⁻1.3)", "⁻1.");
		}
		
		[TestMethod]
		public void CeilingOfNegativeFloatTest()
		{
			Common.EvaluateAndAssert("ceiling(⁻2)", "⁻2");
		}

		[TestMethod]
		public void CeilingOfZeroFloatTest()
		{
			Common.EvaluateAndAssert("ceiling(0.0)", "0.");
		}

		[TestMethod]
		public void CeilingOfZeroIntegerTest()
		{
			Common.EvaluateAndAssert("ceiling(0)", "0");
		}

		[TestMethod]
		public void CeilingOfPositiveIntegerTest()
		{
			Common.EvaluateAndAssert("ceiling(2)", "2");
		}

		[TestMethod]
		public void CeilingOfPositiveFloatTest()
		{
			Common.EvaluateAndAssert("ceiling(1.3)", "2.");
		}
	}
}
