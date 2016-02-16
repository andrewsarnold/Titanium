using System.Linq;
using Titanium.Core.Exceptions;
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
		internal readonly ComponentType ComponentType;

		public DualFactorComponent(Factor leftFactor, Factor rightFactor, ComponentType componentType)
		{
			ComponentType = componentType;
			LeftFactor = leftFactor;
			RightFactor = rightFactor;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", ToString(LeftFactor),
				ComponentType == ComponentType.Multiply
					? "*"
					: ComponentType == ComponentType.Divide
						? "/"
						: "^",
				ToString(RightFactor, ComponentType == ComponentType.Divide));
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
				return Evaluate(leftList, right, ComponentType);
			}

			if (Common.IsList(right, out rightList))
			{
				return Evaluate(rightList, left, ComponentType);
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsNumber(left, out leftNumber) && Common.IsIntegerFraction(right, out rightFraction))
			{
				// Special case: negative left ^ fractional right (any number with a decimal portion) is non-real
				if (leftNumber.IsNegative && rightFraction.Denominator > 1)
				{
					throw new NonRealResultException();
				}

				return Expressionizer.ToExpression(ComponentType == ComponentType.Multiply
					? leftNumber * rightFraction
					: ComponentType == ComponentType.Divide
						? leftNumber / rightFraction
						: leftNumber ^ rightFraction);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				// Special case: negative left ^ fractional right (any number with a decimal portion) is non-real
				if (leftFraction.IsNegative && !Number.IsWholeNumber(rightNumber))
				{
					throw new NonRealResultException();
				}

				return Expressionizer.ToExpression(ComponentType == ComponentType.Multiply
					? leftFraction * rightNumber
					: ComponentType == ComponentType.Divide
						? leftFraction / rightNumber
						: leftFraction ^ rightNumber);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				// Special case: negative left ^ fractional right (any number with a decimal portion) is non-real
				if (leftFraction.IsNegative && rightFraction.Denominator > 1)
				{
					throw new NonRealResultException();
				}

				return Expressionizer.ToExpression(ComponentType == ComponentType.Multiply
					? leftFraction * rightFraction
					: ComponentType == ComponentType.Divide
						? leftFraction / rightFraction
						: leftFraction ^ rightFraction);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				// Special case: negative left ^ fractional right (any number with a decimal portion) is non-real
				if (leftFraction.IsNegative && !Number.IsWholeNumber(rightNumber))
				{
					throw new NonRealResultException();
				}

				if (rightNumber is Integer)
				{
					var rightInteger = (Integer)rightNumber;
					var result = ComponentType == ComponentType.Multiply
						? leftFraction * rightInteger
						: ComponentType == ComponentType.Divide
							? leftFraction / rightInteger
							: leftFraction ^ rightInteger;
					return Expressionizer.ToExpression(result);
				}
				else
				{
					var rightDouble = (Float)rightNumber;
					var result = ComponentType == ComponentType.Multiply
						? leftFraction * rightDouble
						: ComponentType == ComponentType.Divide
							? leftFraction / rightDouble
							: leftFraction ^ rightDouble;
					return Expressionizer.ToExpression(new NumericFactor(result));
				}
			}

			return Expressionizer.ToExpression(new DualFactorComponent(Factorizer.ToFactor(left), Factorizer.ToFactor(right), ComponentType));
		}

		private static Expression Evaluate(ExpressionList leftNumber, IEvaluatable right, ComponentType type)
		{
			return Expressionizer.ToExpression(new ExpressionList(leftNumber.Expressions.Select(e => new DualFactorComponent(Factorizer.ToFactor(e), Factorizer.ToFactor(right), type).Evaluate()).ToList()));
		}

		private Expression Evaluate(Number leftNumber, Number rightNumber)
		{
			if (ComponentType == ComponentType.Divide)
			{
				var result = leftNumber / rightNumber;
				return result is IntegerFraction
					? Expressionizer.ToExpression((IntegerFraction)result)
					: Expressionizer.ToExpression(new SingleFactorComponent(new NumericFactor((Number)result)));
			}

			return Expressionizer.ToExpression(new NumericFactor(ComponentType == ComponentType.Multiply
				? leftNumber * rightNumber
				: leftNumber ^ rightNumber));
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
