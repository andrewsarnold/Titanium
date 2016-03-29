using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Reducer
{
	internal static class Factorizer
	{
		internal static Factor ToFactor(Evaluatable thing)
		{
			if (thing is Expression)
			{
				return ToFactor((Expression)thing);
			}

			if (thing is Component)
			{
				return ToFactor((Component)thing);
			}

			if (thing is Factor)
			{
				return ToFactor((Factor)thing);
			}

			if (thing is ComponentListFactor)
			{
				return ((ComponentListFactor)thing).Factor;
			}

			throw new UnexpectedTypeException(thing.GetType());
		}

		private static Factor ToFactor(Expression expression)
		{
			return expression is SingleComponentExpression
				? ToFactor(((SingleComponentExpression)expression).Component)
				: new ExpressionFactor(expression);
		}

		private static Factor ToFactor(Component component)
		{
			return component is SingleFactorComponent
				? ToFactor(((SingleFactorComponent)component).Factor)
				: new ExpressionFactor(new SingleComponentExpression(component));
		}

		private static Factor ToFactor(Factor factor)
		{
			return factor is ExpressionFactor
				? ToFactor(((ExpressionFactor)factor).Expression)
				: factor;
		}
	}
}
