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

		internal override Factor Evaluate()
		{
			var result = Expression.Evaluate();
			var singleComponentExpression = result as SingleComponentExpression;
			if (singleComponentExpression != null)
			{
				var component = singleComponentExpression.Component;
				var singleFactorComponent = component as SingleFactorComponent;
				if (singleFactorComponent != null)
				{
					return singleFactorComponent.Factor;
				}
			}

			return new ExpressionFactor(result);
		}
	}
}
