using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class AssignTests
	{
		[TestMethod]
		public void AllowAssigning()
		{
			Common.EvaluateAndAssert("5→x", "5");
		}
	}
}
