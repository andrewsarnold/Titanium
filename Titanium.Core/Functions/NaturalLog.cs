using System;
using System.Collections.Generic;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions
{
	internal class NaturalLog : Function
	{
		public NaturalLog()
			: base("ln", 1)
		{
		}

		protected override Expression InnerEvaluate(List<Expression> parameters)
		{
			var parameter = parameters[0].Evaluate();

			var factor = Factorizer.ToFactor(parameter);
			if (factor is AlphabeticFactor && ((AlphabeticFactor)factor).Value == "e")
			{
				return Expressionizer.ToExpression(new NumericFactor(new Integer(1)));
			}

			if (factor is NumericFactor)
			{
				var number = ((NumericFactor)factor).Number;
				
				if (number.IsNegative)
				{
					throw new NonRealResultException();
				}

				if (number is Float)
				{
					return Expressionizer.ToExpression(new NumericFactor(new Float(Math.Log(((Float)number).Value))));
				}

				if (number is Integer)
				{
					var result = Math.Log(((Integer)number).Value);
					if (Float.IsWholeNumber(result))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
					}
				}
			}

			return AsExpression(parameter);
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("{0}({1})", Name, parameters[0]);
		}
	}
}
