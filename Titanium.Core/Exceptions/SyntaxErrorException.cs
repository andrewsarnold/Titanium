using System;

namespace Titanium.Core.Exceptions
{
	internal class SyntaxErrorException : Exception
	{
		internal SyntaxErrorException(Exception innerException)
			: base("Syntax error", innerException)
		{
		}

		internal SyntaxErrorException()
			: base("Syntax error")
		{
		}

		internal SyntaxErrorException(string message)
			: base(message)
		{
		}

		internal SyntaxErrorException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}
	}
}
