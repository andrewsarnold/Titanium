using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class MultiplicationTests
	{
		[TestMethod]
		public void IntegerTimesInteger()
		{
			Common.EvaluateAndAssert("2 * 3", "6");
		}

		[TestMethod]
		public void IntegerTimesFloat()
		{
			Common.EvaluateAndAssert("2 * 0.5", "1.");
		}

		[TestMethod]
		public void NegativeIntegerTimesFloat()
		{
			Common.EvaluateAndAssert("⁻2 * 0.5", "⁻1.");
		}

		[TestMethod]
		public void NegativeIntegerTimesNegativeFloat()
		{
			Common.EvaluateAndAssert("⁻2 * ⁻0.5", "1.");
		}
	}
}
