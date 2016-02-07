using System;
using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions
{
	internal class SimpleFloatCalculation : Function
	{
		private readonly Func<double, double> _function;

		public SimpleFloatCalculation(string name, Func<double, double> function)
			: base(name, 1)
		{
			_function = function;
		}

		public override Expression Evaluate(List<Expression> parameters)
		{
			if (parameters.Count != 1)
			{
				throw new WrongArgumentCountException(Name, 1, parameters.Count);
			}

			var factor = Factorizer.ToFactor(parameters[0].Evaluate());
			if (factor is NumericFactor)
			{
				var number = (NumericFactor)factor;
				if (number.Number is Integer)
				{
					var integer = (Integer)number.Number;
					var result = _function(integer.Value);
					if (Float.IsWholeNumber(result))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
					}

					return Expressionizer.ToExpression(new FunctionComponent(Name, new List<Expression> { Expressionizer.ToExpression(number) }));
				}
				
				var aFloat = (Float)number.Number;
				return Expressionizer.ToExpression(new NumericFactor(new Float(_function(aFloat.Value))));
			}
			
			if (factor is AlphabeticFactor)
			{
				// If operand is a constant (like pi), and the result is not an integer, don't evaluate
				var alph = (AlphabeticFactor)factor;
				if (Constants.IsNamedConstant(alph.Value))
				{
					var result = _function(Constants.Get(alph.Value));
					if (Float.IsWholeNumber(result))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
					}
					return Expressionizer.ToExpression(new FunctionComponent(Name, parameters.ToList()));
				}
			}

			return Expressionizer.ToExpression(new FunctionComponent(Name, parameters.ToList()));
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("{0}({1})", Name, string.Join(",", parameters));
		}
	}
}
