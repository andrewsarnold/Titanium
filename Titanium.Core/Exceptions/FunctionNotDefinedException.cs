using System;

namespace Titanium.Core.Exceptions
{
	internal class FunctionNotDefinedException : Exception
	{
		public FunctionNotDefinedException(string functionName)
			: base(string.Format("Function {0} not defined", functionName))
		{
		}
	}
}
