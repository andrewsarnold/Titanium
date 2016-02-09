using System;
using System.Collections.Generic;
using Titanium.Core.Components;
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

				if (number.IsZero)
				{
					return Expressionizer.ToExpression(new NumericFactor(new Infinity(true)));
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

			var component = Componentizer.ToComponent(parameter);
			if (component is DualFactorComponent)
			{
				var dfc = (DualFactorComponent)component;
				switch (dfc.ComponentType)
				{
					case ComponentType.Multiply:
						return EvaluateMultiplication(dfc.LeftFactor, dfc.RightFactor);
					case ComponentType.Divide:
						return EvaluateDivision(dfc.LeftFactor, dfc.RightFactor);
					case ComponentType.Exponent:
						return EvaluateExponent(dfc.LeftFactor, dfc.RightFactor);
				}
			}

			return AsExpression(parameter);
		}

		private Expression EvaluateMultiplication(Factor left, Factor right)
		{
			// ln(a * b) = ln(a) + ln(b)
			return new DualComponentExpression(
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { left.Evaluate() }).Evaluate()),
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { right.Evaluate() }).Evaluate()),
				true);
		}

		private Expression EvaluateDivision(Factor left, Factor right)
		{
			// ln(a / b) = ln(a) - ln(b)
			return new DualComponentExpression(
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { left.Evaluate() }).Evaluate()),
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { right.Evaluate() }).Evaluate()),
				false);
		}

		private Expression EvaluateExponent(Factor left, Factor right)
		{
			// ln(a ^ b) = b * ln(a)
			return new DualFactorComponent(
				Factorizer.ToFactor(right.Evaluate()),
				Factorizer.ToFactor(new FunctionComponent(Name, new List<Expression> { left.Evaluate() })),
				ComponentType.Multiply).Evaluate();
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("{0}({1})", Name, parameters[0]);
		}
	}
}
