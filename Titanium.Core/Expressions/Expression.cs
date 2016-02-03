using System;
using Titanium.Core.Exceptions;

namespace Titanium.Core.Expressions
{
	public abstract class Expression
	{
		public static Expression ParseExpression(string input)
		{
			try
			{
				return ExpressionParser.Parse(input);
			}
			catch (Exception e)
			{
				throw new SyntaxErrorException(e);
			}
		}

		public abstract Expression Evaluate();
	}
}
