using System;

namespace Titanium.Core.Exceptions
{
	internal class DomainException : Exception
	{
		public DomainException()
			: base("Domain error")
		{
		}
	}
}
