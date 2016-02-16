using Titanium.Core.Components;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core
{
	internal static class Common
	{
		internal static bool IsNumber(IEvaluatable evaluatable, out Number number)
		{
			return IsNumber(Componentizer.ToComponent(evaluatable), out number);
		}

		internal static bool IsIntegerFraction(IEvaluatable evaluatable, out IntegerFraction fraction)
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

		internal static bool IsFloat(IEvaluatable evaluatable, out Number number)
		{
			return IsNumber(evaluatable, out number) && number is Float;
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

		public static bool IsFunction(IEvaluatable evaluatable, out FunctionComponent function)
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
	}
}
