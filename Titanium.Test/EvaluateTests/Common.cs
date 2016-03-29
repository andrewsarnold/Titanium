using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core;

namespace Titanium.Test.EvaluateTests
{
	internal static class Common
	{
		internal static void EvaluateAndAssert(string input, string output)
		{
			Assert.AreEqual(output, Evaluator.Evaluate(input));
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
