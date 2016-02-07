﻿using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions
{
	internal class Negate : Function
	{
		public Negate()
			: base("⁻", 1)
		{
		}

		public override Expression Evaluate(List<Expression> parameters)
		{
			var p = parameters[0].Evaluate();
			if (p is SingleComponentExpression)
			{
				var c = ((SingleComponentExpression)p).Component;
				if (c is SingleFactorComponent)
				{
					var f = ((SingleFactorComponent)c).Factor;
					if (f is NumericFactor)
					{
						var n = ((NumericFactor)f).Number;
						return Expressionizer.ToExpression(new NumericFactor(Integer.Zero - n));
					}
				}
				else if (c is IntegerFraction)
				{
					var f = ((IntegerFraction)c);
					return Expressionizer.ToExpression(new IntegerFraction(new Integer(-f.Numerator), new Integer(f.Denominator)));
				}
			}
			
			return Expressionizer.ToExpression(new FunctionComponent(this, new List<Expression> { p }));
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("⁻{0}",
				parameters[0] is DualComponentExpression
					? string.Format("({0})", parameters[0])
					: parameters[0].ToString());
		}
	}
}