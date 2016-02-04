using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Reducer
{
	internal static class Expressionizer
	{
		internal static Expression ToExpression(IEvaluatable thing)
		{
			if (thing is Expression)
			{
				return ToExpression((Expression)thing);
			}

			if (thing is Component)
			{
				return ToExpression((Component)thing);
			}

			if (thing is Factor)
			{
				return ToExpression((Factor)thing);
			}

			throw new UnexpectedTypeException(thing.GetType());
		}

		private static Expression ToExpression(Expression expression)
		{
			return expression is SingleComponentExpression
				? ToExpression(((SingleComponentExpression)expression).Component)
				: expression;
		}

		private static Expression ToExpression(Component component)
		{
			return component is SingleFactorComponent
				? ToExpression(((SingleFactorComponent)component).Factor)
				: new SingleComponentExpression(component);
		}

		private static Expression ToExpression(Factor factor)
		{
			return factor is ExpressionFactor
				? ToExpression(((ExpressionFactor)factor).Expression)
				: new SingleComponentExpression(new SingleFactorComponent(factor));
		}
	}
}