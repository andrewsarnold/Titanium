using System;
using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class SquareRoot : Function
	{
		public SquareRoot()
			: base("√", 1)
		{
		}

		protected override Expression InnerEvaluate(List<Expression> parameters)
		{
			var parameter = parameters[0].Evaluate();
			return new Exponent().Evaluate(new List<Expression> { parameter, Expressionizer.ToExpression(new IntegerFraction(1, 2)) });

			var factor = Factorizer.ToFactor(parameter);
			if (factor is NumericFactor)
			{
				var number = ((NumericFactor)factor).Number;
				if (number.IsNegative)
				{
					throw new NonRealResultException();
				}

				if (number is Float)
				{
					var f = ((Float)number).Value;
					return Expressionizer.ToExpression(new NumericFactor(new Float(Math.Sqrt(f))));
				}

				var i = ((Integer)number).Value;
				var result = Math.Sqrt(i);
				if (Float.IsWholeNumber(result))
				{
					return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
				}
			}

			return AsExpression(parameter);
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("√({0})", parameters[0]);
		}
	}
}
