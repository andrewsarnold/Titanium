using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ListTests
	{
		[TestMethod]
		public void EvaluateSingleElementList()
		{
			Common.EvaluateAndAssert("{2*2}", "{4}");
		}
	}
}
