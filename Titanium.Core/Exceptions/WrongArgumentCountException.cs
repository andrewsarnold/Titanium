using System;

namespace Titanium.Core.Exceptions
{
	internal class WrongArgumentCountException : Exception
	{
		public WrongArgumentCountException(string functionName, int expected, int actual)
			: base(string.Format("Function {0} expected {1} arguments; got {2}", functionName, expected, actual))
		{
		}
	}
}