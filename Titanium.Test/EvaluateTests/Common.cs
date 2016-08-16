using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Evaluator;

namespace Titanium.Test.EvaluateTests
{
	internal static class Common
	{
		internal static void EvaluateAndAssert(string input, string output)
		{
			Assert.AreEqual(output, new Evaluator().Evaluate(input));
		}

		internal static void EvaluateAndAssert(Evaluator evaluator, string input, string output)
		{
			Assert.AreEqual(output, evaluator.Evaluate(input));
		}

		internal static void AssertThrows<T>(string input)
		{
			try
			{
				EvaluateAndAssert(input, string.Empty);
				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex.GetType() == typeof(T));
			}
		}
	}
}
