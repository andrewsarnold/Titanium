using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core
{
	internal static class Common
	{
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
		
		internal static bool IsIntegerFraction(Expression expression, out IntegerFraction fraction)
		{
			var component = Componentizer.ToComponent(expression);
			if (component is IntegerFraction)
			{
				fraction = (IntegerFraction)component;
				return true;
			}

			var factor = Factorizer.ToFactor(expression);
			if (factor is NumericFactor)
			{
				var number = ((NumericFactor)factor).Number;
				if (number is Integer)
				{
					fraction = new IntegerFraction((Integer)number);
					return true;
				}
			}

			fraction = null;
			return false;
		}

		internal static bool IsFloat(Expression expression, out Number number)
		{
			return IsNumber(expression, out number) && number is Float;
		}

		private static bool IsNumber(Component component, out Number number)
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

		private static bool IsIntegerFraction(Component component, out IntegerFraction fraction)
		{
			if (component is IntegerFraction)
			{
				fraction = (IntegerFraction)component;
				return true;
			}

			fraction = null;
			return false;
		}
	}
}
