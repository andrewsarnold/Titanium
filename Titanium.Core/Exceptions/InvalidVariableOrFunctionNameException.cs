using System;

namespace Titanium.Core.Exceptions
{
	internal class InvalidVariableOrFunctionNameException : Exception
	{
		internal InvalidVariableOrFunctionNameException()
			: base("Invalid variable or function name")
		{
		}
	}
}
