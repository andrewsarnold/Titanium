using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
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

		internal override Component Evaluate(List<object> operands)
		{
			if (operands.Count != 1)
			{
				throw new WrongArgumentCountException("!", 1, operands.Count);
			}

			if (operands[0] is NumericFactor)
			{
				var operand = (NumericFactor)operands[0];
				if (operand.Number is Integer)
				{
					var number = (Integer)operand.Number;
					return new IntegerFraction(new Integer(BasicFactorial(number.Value)));
				}
			}
			else if (operands[0] is IntegerFraction)
			{
				var operand = (IntegerFraction)operands[0];
				if (operand.Denominator == 1)
				{
					return new IntegerFraction(new Integer(BasicFactorial(operand.Numerator)));
				}
			}

			return new FunctionComponent("!", operands);
		}

		private static int BasicFactorial(int i)
		{
			if (i <= 1) return 1;
			return i * BasicFactorial(i - 1);
		}
	}
}