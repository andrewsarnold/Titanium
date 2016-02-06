using System.Collections.Generic;
using Titanium.Core.Components;
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

		protected override Expression InnerEvaluate(List<Expression> parameters)
		{
			var parameter = parameters[0].Evaluate();

			if (parameter is SingleComponentExpression)
			{
				var component = ((SingleComponentExpression)parameter).Component;
				if (component is SingleFactorComponent)
				{
					var factor = ((SingleFactorComponent)component).Factor;
					if (factor is NumericFactor)
					{
						var operand = (NumericFactor)factor;
						if (operand.Number is Integer)
						{
							var number = (Integer)operand.Number;
							return Expressionizer.ToExpression(new NumericFactor(new Integer(BasicFactorial(number.Value))));
						}
					}
				}

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
			}

			return AsExpression(parameters);
		}

		public override string ToString(List<Expression> parameters)
		{
			var parameterAsFactor = Factorizer.ToFactor(parameters[0]);
			var shouldWrap = parameterAsFactor is ExpressionFactor ||
				(parameterAsFactor is NumericFactor && ((NumericFactor)parameterAsFactor).Number is Float);
			return string.Format("{0}{1}{2}!",
				shouldWrap ? "(" : string.Empty,
				parameters[0],
				shouldWrap ? ")" : string.Empty);
		}

		private static int BasicFactorial(int i)
		{
			if (i <= 1) return 1;
			return i * BasicFactorial(i - 1);
		}
	}
}
