using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core.Reducer
{
	internal static class Expressionizer
	{
		internal static Expression ToExpression(Evaluatable thing)
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

			if (thing is ComponentListFactor)
			{
				var clf = (ComponentListFactor) thing;
				if (clf.IsInNumerator)
				{
					return ToExpression(clf.Factor);
				}

				return ToExpression(new ComponentList(new List<ComponentListFactor> { clf }));
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
			if (component is ComponentList)
			{
				return ToExpression((ComponentList) component);
			}

			return component is SingleFactorComponent
				? ToExpression(((SingleFactorComponent)component).Factor)
				: new SingleComponentExpression(component);
		}

		private static Expression ToExpression(ComponentList componentList)
		{
			if (componentList.Factors.Count == 1)
			{
				if (componentList.Factors[0].IsInNumerator)
				{
					return ToExpression(componentList.Factors[0].Factor);
				}

				if (componentList.Factors[0].Factor is NumericFactor && !componentList.Factors[0].IsInNumerator)
				{
					var nf = (NumericFactor)componentList.Factors[0].Factor;
					if (nf.Number is Integer)
					{
						return new SingleComponentExpression(new IntegerFraction(new Integer(1), (Integer)nf.Number));
					}
				}
			}

			return new SingleComponentExpression(componentList);
		}

		private static Expression ToExpression(Factor factor)
		{
			return factor is ExpressionFactor
				? ToExpression(((ExpressionFactor)factor).Expression)
				: new SingleComponentExpression(new SingleFactorComponent(factor));
		}
	}
}