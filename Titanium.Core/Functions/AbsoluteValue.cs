using System;
using System.Collections.Generic;
using Titanium.Core.Expressions;

namespace Titanium.Core.Functions
{
	internal class AbsoluteValue : Function
	{
		public AbsoluteValue()
			: base("abs", 1)
		{
		}

		protected override Expression InnerEvaluate(List<Expression> parameters)
		{
			throw new NotImplementedException();
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("|{0}|", parameters[0]);
		}
	}
}
