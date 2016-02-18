using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Reducer
{
	internal static class Componentizer
	{
		internal static Component ToComponent(Evaluatable thing)
		{
			if (thing is Expression)
			{
				return ToComponent((Expression)thing);
			}

			if (thing is Component)
			{
				return ToComponent((Component)thing);
			}

			if (thing is Factor)
			{
				return ToComponent((Factor)thing);
			}

			throw new UnexpectedTypeException(thing.GetType());
		}

		private static Component ToComponent(Expression expression)
		{
			return expression is SingleComponentExpression
				? ToComponent(((SingleComponentExpression)expression).Component)
				: new SingleFactorComponent(new ExpressionFactor(expression));
		}

		private static Component ToComponent(Component component)
		{
			return component is SingleFactorComponent
				? ToComponent(((SingleFactorComponent)component).Factor)
				: component;
		}

		private static Component ToComponent(Factor factor)
		{
			return factor is ExpressionFactor
				? ToComponent(((ExpressionFactor)factor).Expression)
				: new SingleFactorComponent(factor);
		}
	}
}
