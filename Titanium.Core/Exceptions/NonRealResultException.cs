using System;

namespace Titanium.Core.Exceptions
{
	internal class NonRealResultException : Exception
	{
		internal NonRealResultException()
			: base("Non-real result")
		{
		}
	}
}
