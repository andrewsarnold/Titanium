using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core;

namespace Titanium.Test
{
	[TestClass]
	public class StateTests
	{
		[TestMethod]
		public void HistoryTest()
		{
			var ev = new Evaluator();
			ev.Evaluate("3.14");

			var history = ev.Evaluate("ans(1)");
			Assert.AreEqual("3.14", history);

			ev.Evaluate("4.0");
			ev.Evaluate("7.4");

			history = ev.Evaluate("ans(2)");
			Assert.AreEqual("4.", history);
		}
	}
}
