using System;

namespace Titanium.Core.Exceptions
{
	internal class IncomparableTypeException : Exception
	{
		public IncomparableTypeException(Type first, Type second)
			: base(string.Format("Can't compare objects of types {0} and {1}", first.Name, second.Name))
		{
		}
	}
}
