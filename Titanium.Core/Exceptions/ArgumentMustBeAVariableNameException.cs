using System;

namespace Titanium.Core.Exceptions
{
	internal class ArgumentMustBeAVariableNameException : Exception
	{
		internal ArgumentMustBeAVariableNameException()
			: base("Argument must be a variable name")
		{
		}
	}
}
