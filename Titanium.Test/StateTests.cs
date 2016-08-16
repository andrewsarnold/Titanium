using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core;
using Titanium.Core.Exceptions;

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

			EvaluateAndAssert(ev, "ans(1)", "3.14");

			ev.Evaluate("4.0");
			ev.Evaluate("7.4");

			EvaluateAndAssert(ev, "ans(2)", "4.");
		}

		[TestMethod]
		public void HistoryOutOfBoundsTest()
		{
			var ev = new Evaluator();
			ev.Evaluate("3.14");

			AssertThrows<DomainException>(ev, "ans(2)");
		}
		
		private static void EvaluateAndAssert(Evaluator e, string input, string output)
		{
			Assert.AreEqual(output, e.Evaluate(input));
		}

		private static void AssertThrows<T>(Evaluator e, string input)
		{
			try
			{
				EvaluateAndAssert(e, input, string.Empty);
				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex.GetType() == typeof(T));
			}
		}
	}
}
