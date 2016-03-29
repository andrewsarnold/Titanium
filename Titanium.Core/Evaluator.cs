using Titanium.Core.Expressions;

namespace Titanium.Core
{
	public static class Evaluator
	{
		public static string Evaluate(string input)
		{
			return Expression.ParseExpression(input).Evaluate().ToString();
		}
	}
}
