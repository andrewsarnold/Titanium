using System;
using System.Collections.Generic;
using Titanium.Core.Exceptions;

namespace Titanium.Core.Expressions
{
	internal abstract class Expression : Evaluatable
	{
		internal static Expression ParseExpression(string input, Dictionary<string, Expression> variableMap = null)
		{
			try
			{
				return ExpressionParser.Parse(input, variableMap);
			}
			catch (Exception e)
			{
				throw new SyntaxErrorException(e);
			}
		}
	}
}
