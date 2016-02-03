using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class AbsoluteValueTests
	{
		[TestMethod]
		public void AbsoluteValueTest()
		{
			Common.EvaluateAndAssert("abs(3)", "3");
			Common.EvaluateAndAssert("abs(⁻3)", "3");
			Common.EvaluateAndAssert("abs(0)", "0");
			Common.EvaluateAndAssert("abs(e)", "e");
			Common.EvaluateAndAssert("abs(4/3)", "4/3");
			Common.EvaluateAndAssert("abs(⁻4/3)", "4/3");
			Common.EvaluateAndAssert("abs(3.4)", "3.4");
			Common.EvaluateAndAssert("abs(⁻3.4)", "3.4");
			Common.EvaluateAndAssert("abs(∞)", "∞");
			Common.EvaluateAndAssert("abs(⁻∞)", "∞");
		}
	}
}
