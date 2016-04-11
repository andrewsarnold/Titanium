using System;
using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class BaseTenLogarithm : Function
	{
		internal BaseTenLogarithm()
			: base("log", 1)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			var parameter = parameters[0];

			var factor = Factorizer.ToFactor(parameter);
			if (factor is NumericFactor)
			{
				var number = ((NumericFactor)factor).Number;
				if (number is Integer)
				{
					var integer = (Integer)number;
					// If number is a power of 10, reduce
					if (IsPowerOf(integer.Value, 10))
					{
						var result = Math.Log10(integer.Value);
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
					}
				}
			}

			// Otherwise, conver to natural log and evaluate that
			var numerator = Factorizer.ToFactor(new NaturalLog().Evaluate(parameter).Evaluate());
			var denominator = Factorizer.ToFactor(new NaturalLog().Evaluate(Expressionizer.ToExpression(new NumericFactor(new Integer(10)))).Evaluate());
			return new DualFactorComponent(numerator, denominator, false).Evaluate();
		}

		internal override string ToString(List<Expression> parameters)
		{
			throw new NotImplementedException();
		}

		private static bool IsPowerOf(int x, int y)
		{
			var d = Math.Log(Math.Abs(x)) / Math.Log(Math.Abs(y));

			if ((x > 0 && y > 0) || (x < 0 && y < 0))
			{
				return Math.Abs(d - (int)d) < Constants.Tolerance;
			}
			
			if (x > 0 && y < 0)
			{
				return (int)d % 2 == 0;
			}
			
			return false;
		}
	}
}
