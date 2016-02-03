using System;

namespace Titanium.Core.Exceptions
{
	public class SyntaxErrorException : Exception
	{
		public SyntaxErrorException(Exception innerException)
			: base("Syntax error", innerException)
		{
		}

		public SyntaxErrorException(string message)
			: base(message)
		{
		}

		public SyntaxErrorException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}
	}
}
