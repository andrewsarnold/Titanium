using System;
using Titanium.Core.Exceptions;

namespace Titanium.Core.Expressions
{
	public abstract class Expression : IEvaluatable
	{
		public static Expression ParseExpression(string input)
		{
			return ExpressionParser.Parse(input);
			try
			{
			}
			catch (Exception e)
			{
				throw new SyntaxErrorException(e);
			}
		}

		public abstract Expression Evaluate();
	}
}
