using System;
using Titanium.Core.Exceptions;

namespace Titanium.Core.Expressions
{
	internal abstract class Expression : Evaluatable
	{
		internal static Expression ParseExpression(string input)
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
	}
}
