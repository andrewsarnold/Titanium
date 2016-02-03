using System;

namespace Titanium.Core.Exceptions
{
	internal class UnexpectedTypeException : Exception
	{
		public UnexpectedTypeException(Type type)
			: base(string.Format("Wasn't expecting object of type {0}", type.Name))
		{
		}
	}
}
