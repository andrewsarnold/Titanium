using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core.Functions.Implementations
{
	internal class Factorial : Function
	{
		public Factorial()
			: base("!")
		{
		}

		public override Expression Evaluate(List<Expression> parameters)
		{
			if (parameters.Count != 1)
			{
				throw new WrongArgumentCountException("!", 1, parameters.Count);
			}

			if (parameters[0] is SingleComponentExpression)
			{
				var component = ((SingleComponentExpression)parameters[0]).Component;
				if (component is SingleFactorComponent)
				{
					var factor = ((SingleFactorComponent)component).Factor;
					if (factor is NumericFactor)
					{
						var operand = (NumericFactor)factor;
						if (operand.Number is Integer)
						{
							var number = (Integer)operand.Number;
							return new SingleComponentExpression(new IntegerFraction(new Integer(BasicFactorial(number.Value))));
						}
					}
				}
				else if (component is IntegerFraction)
				{
					var operand = (IntegerFraction)component;
					if (operand.Denominator == 1)
					{
						return new SingleComponentExpression(new IntegerFraction(new Integer(BasicFactorial(operand.Numerator))));
					}
				}
			}

			return new SingleComponentExpression(new FunctionComponent("!", parameters.Cast<IEvaluatable>().ToList()));
		}

		private static int BasicFactorial(int i)
		{
			if (i <= 1) return 1;
			return i * BasicFactorial(i - 1);
		}
	}
}