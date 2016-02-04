using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class ExpressionList : Factor
	{
		private readonly IEnumerable<Expression> _expressions;

		public ExpressionList(IEnumerable<Expression> expressions)
		{
			_expressions = expressions;
		}

		public override Expression Evaluate()
		{
			return Expressionizer.ToExpression(this);
		}

		public override string ToString()
		{
			return string.Format("{{{0}}}", string.Join(",", _expressions));
		}
	}
}
