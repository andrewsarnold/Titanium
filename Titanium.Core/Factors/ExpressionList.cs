using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class ExpressionList : Factor
	{
		internal List<Expression> Expressions;

		public ExpressionList(List<Expression> expressions)
		{
			Expressions = expressions;
		}

		public override Expression Evaluate()
		{
			Expressions = Expressions.Select(e => e.Evaluate()).ToList();
			return Expressionizer.ToExpression(this);
		}

		public override string ToString()
		{
			return string.Format("{{{0}}}", string.Join(",", Expressions));
		}
	}
}
