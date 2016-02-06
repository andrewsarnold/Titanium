using System;
using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions
{
	internal class AbsoluteValue : Function
	{
		public AbsoluteValue()
			: base("abs", 1)
		{
		}

		protected override Expression InnerEvaluate(List<Expression> parameters)
		{
			var p = parameters[0].Evaluate();
			var c = Componentizer.ToComponent(p);
			var f = Factorizer.ToFactor(p);

			// If parameter is a number, get the raw absolute value
			if (f is NumericFactor)
			{
				var n = ((NumericFactor)f).Number;
				return Expressionizer.ToExpression(new NumericFactor(n is Integer
					? new Integer(Math.Abs((((Integer)n).Value)))
					: (Number)new Float(Math.Abs((((Float)n).Value)))));
			}
			
			// If parameter is an integer fraction, likewise
			if (c is IntegerFraction)
			{
				var frac = (IntegerFraction)c;
				return Expressionizer.ToExpression(new IntegerFraction(new Integer(Math.Abs(frac.Numerator)), new Integer(frac.Denominator)));
			}

			// If parameter is a constant, return itself if it evaluates to a positive number
			if (f is AlphabeticFactor)
			{
				var a = ((AlphabeticFactor)f).Value;
				if (Constants.IsNamedConstant(a))
				{
					var constant = Constants.Get(a);
					if (constant >= 0)
					{
						return p;
					}
				}
			}

			if (c is FunctionComponent)
			{
				// TODO - See if the function evaluates to a negative value; if not, return it
			}

			// Otherwise just return this
			return AsExpression(parameters);
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("|{0}|", parameters[0]);
		}
	}
}
