using System;
using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core.Functions
{
	internal class Trigonometric : Function
	{
		private readonly Func<double, double> _function;

		public Trigonometric(string name, Func<double, double> function)
			: base(name)
		{
			_function = function;
		}

		internal override Component Evaluate(List<object> parameters)
		{
			if (parameters.Count != 1)
			{
				throw new WrongArgumentCountException(Name, 1, parameters.Count);
			}

			if (parameters[0] is AlphabeticFactor)
			{
				// If operand is a constant (like pi), and the result is not an integer, don't evaluate
				var factor = (AlphabeticFactor)parameters[0];
				if (Constants.IsNamedConstant(factor.Value))
				{
					var result = _function(Constants.Get(factor.Value));
					if (IsInteger(result))
					{
						return new SingleFactorComponent(new NumericFactor(new Integer((int)result)));
					}
					return new FunctionComponent(Name, parameters);
				}
			}

			var operand = Reducer.GetFactor(parameters[0]);
			if (operand is NumericFactor)
			{
				var factor = (NumericFactor)operand;
				if (factor.Number is Integer)
				{
					var number = (Integer)factor.Number;
					var result = _function(number.Value);
					if (IsInteger(result))
					{
						return new SingleFactorComponent(new NumericFactor(new Integer((int)result)));
					}
				}
				else
				{
					var number = (Float)factor.Number;
					return new SingleFactorComponent(new NumericFactor(new Float(_function(number.Value))));
				}
			}

			return new FunctionComponent(Name, parameters);
		}

		private static bool IsInteger(double d)
		{
			return Math.Abs(d % 1) < Constants.Tolerance;
		}
	}
}