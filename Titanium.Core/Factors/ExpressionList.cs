using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class ExpressionList : Factor
	{
		internal List<Expression> Expressions;

		internal ExpressionList(List<Expression> expressions)
		{
			Expressions = expressions;
		}

		internal override Expression Evaluate()
		{
			Expressions = Expressions.Select(e => e.Evaluate()).ToList();
			return Expressionizer.ToExpression(this);
		}

		public override string ToString()
		{
			return string.Format("{{{0}}}", string.Join(",", Expressions));
		}

		public override int CompareTo(object obj)
		{
			throw new System.NotImplementedException();
		}
	}
}
