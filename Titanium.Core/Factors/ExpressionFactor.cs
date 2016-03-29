using Titanium.Core.Components;
using Titanium.Core.Expressions;

namespace Titanium.Core.Factors
{
	internal class ExpressionFactor : Factor
	{
		internal readonly Expression Expression;

		internal ExpressionFactor(Expression expression)
		{
			Expression = expression;
		}

		public override string ToString()
		{
			return Expression.ToString();
		}

		internal override Expression Evaluate()
		{
			return Expression.Evaluate();
		}
	}
}
