using System.Collections.Generic;
using Titanium.Core.Expressions;

namespace Titanium.Core.Functions
{
	internal class Expand : Function
	{
		public Expand()
			: base("expand", 1)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			return parameters[0].Evaluate(true);
		}

		protected override Expression InnerExpand(params Expression[] parameters)
		{
			throw new System.NotImplementedException();
		}

		internal override string ToString(List<Expression> parameters)
		{
			return parameters[0].ToString();
		}
	}
}
