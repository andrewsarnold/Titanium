using System.Collections.Generic;
using Titanium.Core.Expressions;

namespace Titanium.Core.Functions
{
	internal class NaturalLog : Function
	{
		public NaturalLog()
			: base("ln", 1)
		{
		}

		protected override Expression InnerEvaluate(List<Expression> parameters)
		{
			return AsExpression(parameters[0]);
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("{0}({1})", Name, parameters[0]);
		}
	}
}
