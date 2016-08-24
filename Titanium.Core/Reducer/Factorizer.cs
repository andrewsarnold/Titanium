using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Functions.Implementations;

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
				var clf = (ComponentListFactor)thing;
				if (clf.IsInNumerator)
				{
					return clf.Factor;
				}

				return new ExpressionFactor(new SingleComponentExpression(new ComponentList(new List<ComponentListFactor> { clf })));
			}

			if (thing is ExpressionListComponent)
			{
				var elc = (ExpressionListComponent)thing;

				var asFactor = ToFactor(elc.Component);
				if (asFactor is NumericFactor)
				{
					var factor = (NumericFactor)asFactor;
					return new NumericFactor(elc.IsAdd ? factor.Number : factor.Number.Negate());
				}

				if (elc.IsAdd)
				{
					return ToFactor(elc.Component);
				}

				return new ExpressionFactor(new ExpressionList(new List<ExpressionListComponent> { elc }));
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
			var functionComponent = component as FunctionComponent;
			if (functionComponent?.Function is Negate)
			{
				var operand = ToFactor(functionComponent.Operands[0]);
				if (operand is NumericFactor)
				{
					return new NumericFactor(((NumericFactor)operand).Number.Negate());
				}
			}

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
