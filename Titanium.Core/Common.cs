using Titanium.Core.Components;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core
{
	internal static class Common
	{
		internal static bool IsFunction(Evaluatable evaluatable, out FunctionComponent function)
		{
			var component = Componentizer.ToComponent(evaluatable);
			if (component is FunctionComponent)
			{
				function = (FunctionComponent)component;
				return true;
			}

			function = null;
			return false;
		}

		internal static bool IsNumber(Evaluatable evaluatable, out Number number)
		{
			return IsNumber(Componentizer.ToComponent(evaluatable), out number);
		}

		internal static bool IsIntegerFraction(Evaluatable evaluatable, out IntegerFraction fraction)
		{
			var component = Componentizer.ToComponent(evaluatable);
			if (component is IntegerFraction)
			{
				fraction = (IntegerFraction)component;
				return true;
			}

			var factor = Factorizer.ToFactor(evaluatable);
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

		internal static bool IsFloat(Evaluatable evaluatable, out Number number)
		{
			return IsNumber(evaluatable, out number) && number is Float;
		}

		internal static bool IsConstant(Evaluatable expression, out Number value)
		{
			var factor = Factorizer.ToFactor(expression);
			if (factor is AlphabeticFactor)
			{
				var name = ((AlphabeticFactor)factor).Value;

				if (Constants.IsNamedConstant(name))
				{
					value = new Float(Constants.Get(name));
					return true;
				}
			}

			value = Integer.Zero;
			return false;
		}

		internal static bool IsList(Evaluatable input, out ExpressionList output)
		{
			var factor = Factorizer.ToFactor(input);
			if (factor is ExpressionList)
			{
				output = (ExpressionList)factor;
				return true;
			}

			output = null;
			return false;
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
	}
}
