using System;
using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class Trigonometric : Function
	{
		private readonly Func<double, double> _function;

		public Trigonometric(string name, Func<double, double> function)
			: base(name, 1)
		{
			_function = function;
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			return Evaluate(parameters[0]);
		}

		private Expression Evaluate(Expression parameter)
		{
			var operand = Factorizer.ToFactor(parameter.Evaluate());
			if (operand is AlphabeticFactor)
			{
				// If operand is a constant (like pi), and the result is not an integer, don't evaluate
				var factor = (AlphabeticFactor)operand;
				if (Constants.IsNamedConstant(factor.Value))
				{
					var result = _function(Constants.Get(factor.Value));
					if (IsInteger(result))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
					}
					return Expressionizer.ToExpression(new FunctionComponent(Name, new List<Expression> { parameter }));
				}
			}

			var component = Componentizer.ToComponent(operand);
			if (component.Equals(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(new AlphabeticFactor("π")),
				new ComponentListFactor(new NumericFactor(new Integer(2)), false)
			})))
			{
				return Expressionizer.ToExpression(new NumericFactor(new Integer(0)));
			}

			if (operand is NumericFactor)
			{
				var factor = (NumericFactor)operand;
				if (factor.Number is Integer)
				{
					var number = (Integer)factor.Number;
					var result = _function(number.Value);
					if (IsInteger(result))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
					}
				}
				else
				{
					var number = (Float)factor.Number;

					if (Name == "tan" && Math.Abs(number.Value - Math.PI / 2) < Constants.Tolerance)
					{
						return Expressionizer.ToExpression(new ExpressionFactor(new UndefinedExpression()));
					}

					return Expressionizer.ToExpression(new NumericFactor(new Float(_function(number.Value))));
				}
			}

			return Expressionizer.ToExpression(new FunctionComponent(Name, new List<Expression> { Expressionizer.ToExpression(operand) }));
		}

		private static bool IsInteger(double d)
		{
			return Math.Abs(d % 1) < Constants.Tolerance;
		}

		internal override string ToString(List<Expression> parameters)
		{
			return string.Format("{0}({1})", Name, parameters[0]);
		}
	}
}
