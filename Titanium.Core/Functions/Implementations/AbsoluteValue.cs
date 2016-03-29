using System;
using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class AbsoluteValue : Function
	{
		internal AbsoluteValue()
			: base("abs", 1)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			var parameter = parameters[0].Evaluate();
			var component = Componentizer.ToComponent(parameter);
			var factor = Factorizer.ToFactor(parameter);

			// If parameter is a number, get the raw absolute value
			if (factor is NumericFactor)
			{
				var n = ((NumericFactor)factor).Number;
				return Expressionizer.ToExpression(new NumericFactor(n is Integer
					? new Integer(Math.Abs((((Integer)n).Value)))
					: (Number)new Float(Math.Abs((((Float)n).Value)))));
			}
			
			// If parameter is an integer fraction, likewise
			if (component is IntegerFraction)
			{
				var frac = (IntegerFraction)component;
				return Expressionizer.ToExpression(new IntegerFraction(new Integer(Math.Abs(frac.Numerator)), new Integer(frac.Denominator)));
			}

			// If parameter is a constant, return itself if it evaluates to a positive number
			if (factor is AlphabeticFactor)
			{
				var a = ((AlphabeticFactor)factor).Value;
				if (Constants.IsNamedConstant(a))
				{
					var constant = Constants.Get(a);
					if (constant >= 0)
					{
						return parameter;
					}
				}
			}

			if (component is FunctionComponent)
			{
				// TODO - See if the function evaluates to a negative value; if not, return it
			}

			// Otherwise just return this
			return AsExpression(parameter);
		}

		internal override string ToString(List<Expression> parameters)
		{
			return string.Format("abs({0})", parameters[0]);
		}
	}
}
