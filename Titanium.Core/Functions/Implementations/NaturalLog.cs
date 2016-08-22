using System;
using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class NaturalLog : Function
	{
		internal NaturalLog()
			: base("ln", 1)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
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
					var integer = (Integer)number;
					var result = Math.Log((integer).Value);
					if (Float.IsWholeNumber(result))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result)));
					}

					// If integer x can be written as x = y^z, evaluate the exponential version: ln(y^z) = z*ln(y)
					// For now just reduce if it's a square
					if (Float.IsWholeNumber(Math.Sqrt(integer.Value)))
					{
						return EvaluateExponent(new NumericFactor(new Integer((int)Math.Sqrt(integer.Value))), new NumericFactor(new Integer(2)));
					}
				}
			}

			var component = Componentizer.ToComponent(parameter);
			if (component is DualFactorComponent)
			{
				var dfc = (DualFactorComponent)component;
				return dfc.IsMultiply
					? EvaluateMultiplication(dfc.LeftFactor, dfc.RightFactor)
					: EvaluateDivision(dfc.LeftFactor, dfc.RightFactor);
			}

			if (component is FunctionComponent)
			{
				var func = (FunctionComponent)component;
				if (func.Function is SquareRoot)
				{
					var innerOperand = func.Operands[0];
					var exponent = new Exponent().Evaluate(innerOperand, Expressionizer.ToExpression(new IntegerFraction(1, 2)));
					return new NaturalLog().Evaluate(Expressionizer.ToExpression(exponent)).Evaluate();
				}

				if (func.Function is Exponent)
				{
					return EvaluateExponent(func.Operands[0], func.Operands[1]);
				}
			}

			if (component is IntegerFraction)
			{
				var frac = (IntegerFraction)component;
				return EvaluateDivision(new NumericFactor(new Integer(frac.Numerator)), new NumericFactor(new Integer(frac.Denominator)));
			}

			if (component is ComponentList)
			{
				var compList = (ComponentList) component;
				var es = compList.Factors.Where(f => f.Factor.Equals(new AlphabeticFactor("e"))).ToList();

				var integerValue = 0;
				foreach (var e in es)
				{
					compList.Factors.Remove(e);
					integerValue += e.IsInNumerator ? 1 : -1;
				}

				if (integerValue != 0)
				{
					var remainingFunc = AsExpression(Expressionizer.ToExpression(compList)).Evaluate();
					return new DualComponentExpression(
						Componentizer.ToComponent(remainingFunc),
						Componentizer.ToComponent(new NumericFactor(new Integer(integerValue))),
						true).Evaluate();
				}
			}

			return AsExpression(parameter);
		}

		private Expression EvaluateMultiplication(Evaluatable left, Evaluatable right)
		{
			// ln(a * b) = ln(a) + ln(b)
			return new DualComponentExpression(
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { left.Evaluate() }).Evaluate()),
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { right.Evaluate() }).Evaluate()),
				true).Evaluate();
		}

		private Expression EvaluateDivision(Evaluatable left, Evaluatable right)
		{
			// ln(a / b) = ln(a) - ln(b)
			return new DualComponentExpression(
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { left.Evaluate() }).Evaluate()),
				Componentizer.ToComponent(new FunctionComponent(Name, new List<Expression> { right.Evaluate() }).Evaluate()),
				false).Evaluate();
		}

		private Expression EvaluateExponent(Evaluatable left, Evaluatable right)
		{
			// ln(a ^ b) = b * ln(a)
			return new DualFactorComponent(
				Factorizer.ToFactor(right.Evaluate()),
				Factorizer.ToFactor(new FunctionComponent(Name, new List<Expression> { left.Evaluate() })),
				true).Evaluate();
		}

		internal override string ToString(List<Expression> parameters)
		{
			return string.Format("{0}({1})", Name, parameters[0]);
		}
	}
}
