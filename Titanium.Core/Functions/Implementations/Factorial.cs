using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class Factorial : Function
	{
		public Factorial()
			: base("!", 1, true)
		{
		}

		public override Expression Evaluate(List<Expression> parameters)
		{
			if (parameters.Count != 1)
			{
				throw new WrongArgumentCountException(Name, 1, parameters.Count);
			}

			var parameter = parameters[0].Evaluate();

			var factor = Factorizer.ToFactor(parameter);
			if (factor is NumericFactor)
			{
				var operand = (NumericFactor)factor;
				
				if (operand.Number is Integer)
				{
					if (operand.Number.IsNegative)
					{
						return new UndefinedExpression();
					}
					var number = (Integer)operand.Number;
					return Expressionizer.ToExpression(new NumericFactor(new Integer(BasicFactorial(number.Value))));
				}
				else
				{
					var number = (Float)operand.Number;
					if (Float.IsWholeNumber(number))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Float(BasicFactorial((int)number.Value))));
					}
				}
			}

			var component = Componentizer.ToComponent(parameter);
			if (component is FunctionComponent)
			{
				return Expressionizer.ToExpression(((FunctionComponent)component).Evaluate());
			}

			if (component is IntegerFraction)
			{
				var operand = (IntegerFraction)component;
				if (operand.Denominator == 1)
				{
					return Expressionizer.ToExpression(new IntegerFraction(new Integer(BasicFactorial(operand.Numerator))));
				}
			}

			return Expressionizer.ToExpression(new FunctionComponent(Name, new List<Expression> { parameter }));
		}

		public override string ToString(List<Expression> parameters)
		{
			var parameterAsFactor = Factorizer.ToFactor(parameters[0]);
			var shouldWrap = parameterAsFactor is ExpressionFactor ||
				(parameterAsFactor is NumericFactor && ((NumericFactor)parameterAsFactor).Number is Float);
			return string.Format("{0}{1}{2}{3}",
				shouldWrap ? "(" : string.Empty,
				parameters[0],
				shouldWrap ? ")" : string.Empty,
				Name);
		}

		private static int BasicFactorial(int i)
		{
			if (i <= 1) return 1;
			return i * BasicFactorial(i - 1);
		}
	}
}
