using System;

namespace Titanium.Core.Exceptions
{
	internal class NonRealResultException : Exception
	{
		public NonRealResultException()
			: base("Non-real result")
		{
		}
	}
}
