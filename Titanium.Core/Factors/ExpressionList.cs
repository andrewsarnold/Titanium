using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class ExpressionList : Factor
	{
		private readonly List<Expression> _expressions;

		public ExpressionList(List<Expression> expressions)
		{
			_expressions = expressions;
		}

		public override Expression Evaluate()
		{
			_expressions.ForEach(e => e.Evaluate());
			return Expressionizer.ToExpression(this);
		}

		public override string ToString()
		{
			return string.Format("{{{0}}}", string.Join(",", _expressions));
		}

		internal int Count
		{
			get { return _expressions.Count; }
		}
	}
}
