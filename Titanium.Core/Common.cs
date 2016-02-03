using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core
{
	internal static class Common
	{
		internal static Expression ToExpression(IEvaluatable thing)
		{
			if (thing is Expression)
			{
				return (Expression)thing;
			}

			if (thing is Component)
			{
				return new SingleComponentExpression((Component)thing);
			}

			if (thing is Factor)
			{
				var factor = (Factor)thing;
				return factor is ExpressionFactor
					? ((ExpressionFactor)factor).Expression
					: new SingleComponentExpression(new SingleFactorComponent((Factor)thing));
			}

			throw new UnexpectedTypeException(thing.GetType());
		}

		internal static Component ToComponent(IEvaluatable thing)
		{
			if (thing is Expression)
			{
				var expression = (Expression)thing;
				return expression is SingleComponentExpression
					? ((SingleComponentExpression)expression).Component
					: new SingleFactorComponent(new ExpressionFactor(expression));
			}

			if (thing is Component)
			{
				return (Component)thing;
			}

			if (thing is Factor)
			{
				return new SingleFactorComponent((Factor)thing);
			}

			throw new UnexpectedTypeException(thing.GetType());
		}

		internal static Factor ToFactor(IEvaluatable thing)
		{
			if (thing is Expression)
			{
				var result = ((Expression)thing).Evaluate();
				if (result is SingleComponentExpression)
				{
					var component = ((SingleComponentExpression)result).Component;
					if (component is SingleFactorComponent)
					{
						return ((SingleFactorComponent)component).Factor;
					}
				}

				return new ExpressionFactor(result);
			}

			if (thing is Component)
			{
				var component = (Component)thing;
				return component is SingleFactorComponent
					? ((SingleFactorComponent)component).Factor
					: new ExpressionFactor(new SingleComponentExpression(component));
			}

			if (thing is Factor)
			{
				return (Factor)thing;
			}

			throw new UnexpectedTypeException(thing.GetType());
		}

		internal static bool IsNumber(Factor factor, out Number number)
		{
			if (factor is NumericFactor)
			{
				number = ((NumericFactor)factor).Number;
				return true;
			}

			number = null;
			return false;
		}

		internal static bool IsNumber(Expression expression, out Number number)
		{
			if (expression is SingleComponentExpression)
			{
				var c = ((SingleComponentExpression)expression).Component;
				return IsNumber(c, out number);
			}

			number = null;
			return false;
		}

		internal static bool IsNumber(Component component, out Number number)
		{
			if (component is SingleFactorComponent)
			{
				var sfc = (SingleFactorComponent)component;
				if (sfc.Factor is NumericFactor)
				{
					var nf = (NumericFactor)sfc.Factor;
					number = nf.Number;
					return true;
				}
			}

			if (component is IntegerFraction)
			{
				var inf = (IntegerFraction)component;
				if (inf.Denominator == 1)
				{
					number = new Integer(inf.Numerator * (inf.IsNegative ? -1 : 1));
					return true;
				}
			}

			number = null;
			return false;
		}

		internal static bool IsIntegerFraction(Expression expression, out IntegerFraction fraction)
		{
			if (expression is SingleComponentExpression)
			{
				var component = ((SingleComponentExpression)expression).Component;
				return IsIntegerFraction(component, out fraction);
			}

			fraction = null;
			return false;
		}

		internal static bool IsIntegerFraction(Component component, out IntegerFraction fraction)
		{
			if (component is IntegerFraction)
			{
				fraction = (IntegerFraction)component;
				return true;
			}

			fraction = null;
			return false;
		}

		internal static bool IsIntegerFraction(Factor factor, out IntegerFraction fraction)
		{
			if (factor is ExpressionFactor)
			{
				var expression = ((ExpressionFactor)factor).Expression;
				return IsIntegerFraction(expression, out fraction);
			}

			fraction = null;
			return false;
		}
	}
}
