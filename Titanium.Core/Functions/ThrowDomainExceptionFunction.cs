using System.Collections.Generic;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;

namespace Titanium.Core.Functions
{
	internal class ThrowDomainExceptionFunction : Function
	{
		public ThrowDomainExceptionFunction()
			: base("ans", 1)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			throw new DomainException();
		}

		internal override string ToString(List<Expression> parameters)
		{
			return string.Empty;
		}
	}
}
