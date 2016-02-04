using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Expressions;

namespace Titanium.Test.EvaluateTests
{
	internal static class Common
	{
		internal static void EvaluateAndAssert(string input, string output)
		{
			var expression = Expression.ParseExpression(input);
			var result = expression.Evaluate();
			Assert.AreEqual(output, result.ToString());
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
