using System;
using Titanium.Core.Tokens;

namespace Titanium.Core.Exceptions
{
	public class UnexpectedTokenTypeException : Exception
	{
		public UnexpectedTokenTypeException(TokenType type)
			: base(string.Format("Unexpected token type {0}", type))
		{
		}
	}
}
