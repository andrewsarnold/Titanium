using System;
using System.Collections.Generic;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core.Components
{
	internal class FunctionComponent : Component
	{
		private readonly string _functionName;
		private readonly List<object> _operands;

		public FunctionComponent(string functionName, List<object> operands)
		{
			_functionName = functionName;
			_operands = operands;
		}

		internal override Component Evaluate()
		{
			switch (_functionName)
			{
				case "!":
					return EvaluateFactorial();
				case "sin":
					return SingleArgumentFunction("sin", Math.Sin);
				case "cos":
					return SingleArgumentFunction("cos", Math.Cos);
				case "tan":
					return SingleArgumentFunction("tan", Math.Cos);
			}

			throw new FunctionNotDefinedException(_functionName);
		}

		private Component EvaluateFactorial()
		{
			if (_operands.Count != 1)
			{
				throw new WrongArgumentCountException("!", 1, _operands.Count);
			}
			
			if (_operands[0] is NumericFactor)
			{
				var operand = (NumericFactor) _operands[0];
				if (operand.Number is Integer)
				{
					var number = (Integer) operand.Number;
					return new IntegerFraction(new Integer(Factorial(number.Value)));
				}
			}
			else if (_operands[0] is IntegerFraction)
			{
				var operand = (IntegerFraction) _operands[0];
				if (operand.Denominator == 1)
				{
					return new IntegerFraction(new Integer(Factorial(operand.Numerator)));
				}
			}

			return new FunctionComponent("!", _operands);
		}

		private Component SingleArgumentFunction(string name, Func<double, double> function)
		{
			if (_operands.Count != 1)
			{
				throw new WrongArgumentCountException(name, 1, _operands.Count);
			}
			
			if (_operands[0] is AlphabeticFactor)
			{
				// If operand is a constant (like pi), and the result is not an integer, don't evaluate
				var factor = (AlphabeticFactor)_operands[0];
				if (Constants.IsNamedConstant(factor.Value))
				{
					var result = function(Constants.Get(factor.Value));
					if (IsInteger(result))
					{
						return new SingleFactorComponent(new NumericFactor(new Integer((int)result)));
					}
					return new FunctionComponent(name, _operands);
				}
			}

			var operand = Reducer.GetFactor(_operands[0]);
			if (operand is NumericFactor)
			{
				var factor = (NumericFactor)operand;
				if (factor.Number is Integer)
				{
					var number = (Integer)factor.Number;
					var result = function(number.Value);
					if (IsInteger(result))
					{
						return new SingleFactorComponent(new NumericFactor(new Integer((int)result)));
					}
				}
				else
				{
					var number = (Float)factor.Number;
					return new SingleFactorComponent(new NumericFactor(new Float(function(number.Value))));
				}
			}

			return new FunctionComponent(name, _operands);
		}

		public override string ToString()
		{
			// Special formats
			if (_functionName == "!")
			{
				return string.Format("({0})!", _operands[0]);
			}

			return string.Format("{0}({1})", _functionName, string.Join(",", _operands));
		}

		private static int Factorial(int i)
		{
			if (i <= 1) return 1;
			return i * Factorial(i - 1);
		}

		private static bool IsInteger(double d)
		{
			return Math.Abs(d % 1) < Constants.Tolerance;
		}
	}
}
