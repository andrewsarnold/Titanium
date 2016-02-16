using System.Linq;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	internal class DualFactorComponent : Component
	{
		internal readonly Factor LeftFactor;
		internal readonly Factor RightFactor;
		internal readonly bool IsMultiply;

		public DualFactorComponent(Factor leftFactor, Factor rightFactor, bool isMultiply)
		{
			IsMultiply = isMultiply;
			LeftFactor = leftFactor;
			RightFactor = rightFactor;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", ToString(LeftFactor),
				IsMultiply ? "*" : "/",
				ToString(RightFactor, !IsMultiply));
		}

		public override Expression Evaluate()
		{
			var left = LeftFactor.Evaluate();
			var right = RightFactor.Evaluate();

			Number leftNumber;
			Number rightNumber;

			if ((Common.IsConstant(left, out leftNumber) && Common.IsFloat(right, out rightNumber)) ||
				(Common.IsFloat(left, out leftNumber) && Common.IsConstant(right, out rightNumber)) ||
				(Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber)))
			{
				return Evaluate(leftNumber, rightNumber);
			}

			ExpressionList leftList;
			ExpressionList rightList;

			if (Common.IsList(left, out leftList))
			{
				return Evaluate(leftList, right, IsMultiply);
			}

			if (Common.IsList(right, out rightList))
			{
				return Evaluate(rightList, left, IsMultiply);
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsNumber(left, out leftNumber) && Common.IsIntegerFraction(right, out rightFraction))
			{
				return Expressionizer.ToExpression(IsMultiply
					? leftNumber * rightFraction
					: leftNumber / rightFraction);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				return Expressionizer.ToExpression(IsMultiply
					? leftFraction * rightNumber
					: leftFraction / rightNumber);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				return Expressionizer.ToExpression(IsMultiply
					? leftFraction * rightFraction
					: leftFraction / rightFraction);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				if (rightNumber is Integer)
				{
					var rightInteger = (Integer)rightNumber;
					var result = IsMultiply
						? leftFraction * rightInteger
						: leftFraction / rightInteger;
					return Expressionizer.ToExpression(result);
				}
				else
				{
					var rightDouble = (Float)rightNumber;
					var result = IsMultiply
						? leftFraction * rightDouble
						: leftFraction / rightDouble;
					return Expressionizer.ToExpression(new NumericFactor(result));
				}
			}

			return Expressionizer.ToExpression(new DualFactorComponent(Factorizer.ToFactor(left), Factorizer.ToFactor(right), IsMultiply));
		}

		private static Expression Evaluate(ExpressionList leftNumber, IEvaluatable right, bool isMultiply)
		{
			return Expressionizer.ToExpression(new ExpressionList(leftNumber.Expressions.Select(e => new DualFactorComponent(Factorizer.ToFactor(e), Factorizer.ToFactor(right), isMultiply).Evaluate()).ToList()));
		}

		private Expression Evaluate(Number leftNumber, Number rightNumber)
		{
			if (IsMultiply)
			{
				return Expressionizer.ToExpression(new NumericFactor(leftNumber * rightNumber));
			}

			var result = leftNumber / rightNumber;
			return result is IntegerFraction
				? Expressionizer.ToExpression((IntegerFraction)result)
				: Expressionizer.ToExpression(new SingleFactorComponent(new NumericFactor((Number)result)));
		}

		private static string ToString(IEvaluatable factor, bool isDenominator = false)
		{
			if (factor is ExpressionFactor)
			{
				var expression = Expressionizer.ToExpression(factor);
				if (expression is DualComponentExpression)
				{
					return string.Format("({0})", expression);
				}

				var component = Componentizer.ToComponent(factor);
				if (component is DualFactorComponent || (isDenominator && component is FunctionComponent))
				{
					var output = expression.ToString();
					if (output.Contains("("))
					{
						return string.Format("({0})", expression);
					}
				}
			}

			return factor.ToString();
		}
	}
}
