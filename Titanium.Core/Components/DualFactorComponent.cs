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
				ToString(RightFactor));
		}

		public override Expression Evaluate()
		{
			var left = LeftFactor.Evaluate();
			var right = RightFactor.Evaluate();

			Number leftNumber;
			Number rightNumber;

			if ((IsConstant(left, out leftNumber) && Common.IsFloat(right, out rightNumber)) ||
				(Common.IsFloat(left, out leftNumber) && IsConstant(right, out rightNumber)) ||
				(Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber)))
			{
				return Evaluate(leftNumber, rightNumber);
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				return Expressionizer.ToExpression(ComponentType == ComponentType.Multiply
					? leftFraction * rightFraction
					: ComponentType == ComponentType.Divide
						? leftFraction / rightFraction
						: leftFraction ^ rightFraction);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
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

		private static bool IsConstant(IEvaluatable expression, out Number value)
		{
			var factor = Factorizer.ToFactor(expression);
			if (factor is AlphabeticFactor)
			{
				var name = ((AlphabeticFactor)factor).Value;

				if (Constants.IsNamedConstant(name))
				{
					value = new Float(Constants.Get(name));
					return true;
				}
			}

			value = Integer.Zero;
			return false;
		}

		private static string ToString(Factor factor)
		{
			if (factor is ExpressionFactor)
			{
				var expression = ((ExpressionFactor)factor).Expression;
				if (expression is DualComponentExpression)
				{
					return string.Format("({0})", expression);
				}
			}

			return factor.ToString();
		}
	}
}
