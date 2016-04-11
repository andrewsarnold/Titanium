using System;
using Titanium.Core.Tokens;

namespace Titanium.Core.Exceptions
{
	internal class UnexpectedTokenTypeException : Exception
	{
		internal UnexpectedTokenTypeException(TokenType type)
			: base(string.Format("Unexpected token type {0}", type))
		{
		}
	}
}
