using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Expressions;

namespace Titanium.Test.PrintingTests
{
	public static class Common
	{
		internal static void AssertPrinting(string input, string output)
		{
			Assert.AreEqual(output, Expression.ParseExpression(input).ToString());
		}
	}
}
