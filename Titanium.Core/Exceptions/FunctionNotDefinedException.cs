using System;

namespace Titanium.Core.Exceptions
{
	internal class FunctionNotDefinedException : Exception
	{
		internal FunctionNotDefinedException(string functionName)
			: base(string.Format("Function {0} not defined", functionName))
		{
		}
	}
}
