using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core
{
	internal static class Common
	{
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

		internal static bool IsIntegerFraction(Factor factor, out IntegerFraction fraction)
		{
			if (factor is ExpressionFactor)
			{
				var ef = (ExpressionFactor)factor;
				var ex = ef.Expression;
				if (ex is SingleComponentExpression)
				{
					var sce = (SingleComponentExpression)ex;
					return IsIntegerFraction(sce.Component, out fraction);
				}
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
	}
}
