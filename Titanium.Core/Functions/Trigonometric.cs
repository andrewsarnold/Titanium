using System;
using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
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

		public override Expression Evaluate(List<Expression> parameters)
		{
			if (parameters.Count != 1)
			{
				throw new WrongArgumentCountException(Name, 1, parameters.Count);
			}

			var factor = Common.ToFactor(parameters[0]);
			if (factor is NumericFactor)
			{
				var number = (NumericFactor)factor;
				if (number.Number is Integer)
				{
					var integer = (Integer)number.Number;
					var result = _function(integer.Value);
					if (IsInteger(result))
					{
						return Common.ToExpression(new NumericFactor(new Integer((int)result)));
					}
				}
				else
				{
					var aFloat = (Float)number.Number;
					return Common.ToExpression(new NumericFactor(new Float(_function(aFloat.Value))));
				}
			}
			else if (factor is AlphabeticFactor)
			{
				// If operand is a constant (like pi), and the result is not an integer, don't evaluate
				var alph = (AlphabeticFactor)factor;
				if (Constants.IsNamedConstant(alph.Value))
				{
					var result = _function(Constants.Get(alph.Value));
					if (IsInteger(result))
					{
						return Common.ToExpression(new NumericFactor(new Integer((int)result)));
					}
					return Common.ToExpression(new FunctionComponent(Name, parameters.Cast<IEvaluatable>().ToList()));
				}
			}

			return Common.ToExpression(new FunctionComponent(Name, parameters.Cast<IEvaluatable>().ToList()));
		}

		private static bool IsInteger(double d)
		{
			return Math.Abs(d % 1) < Constants.Tolerance;
		}
	}
}