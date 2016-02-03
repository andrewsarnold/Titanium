using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core.Components
{
	internal class DualFactorComponent : Component
	{
		private readonly Factor _leftFactor;
		private readonly Factor _rightFactor;
		private readonly ComponentType _componentType;

		public DualFactorComponent(Factor leftFactor, Factor rightFactor, ComponentType componentType)
		{
			_componentType = componentType;
			_leftFactor = leftFactor;
			_rightFactor = rightFactor;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", _leftFactor,
				_componentType == ComponentType.Multiply
					? "*"
					: _componentType == ComponentType.Divide
						? "/"
						: "^",
				_rightFactor);
		}

		internal override Component Evaluate()
		{
			var left = _leftFactor.Evaluate();
			var right = _rightFactor.Evaluate();

			Number leftNumber;
			Number rightNumber;

			if (Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber))
			{
				if (_componentType == ComponentType.Divide)
				{
					var result = leftNumber / rightNumber;
					return result is IntegerFraction
						? (Component)(IntegerFraction)result
						: new SingleFactorComponent(new NumericFactor((Number)result));
				}

				return new SingleFactorComponent(new NumericFactor(_componentType == ComponentType.Multiply
					? leftNumber * rightNumber
					: leftNumber ^ rightNumber));
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				return new SingleFactorComponent(new ExpressionFactor(new SingleComponentExpression(_componentType == ComponentType.Multiply
					? leftFraction * rightFraction
					: _componentType == ComponentType.Divide
						? leftFraction / rightFraction
						: leftFraction ^ rightFraction)));
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				if (rightNumber is Integer)
				{
					var rightInteger = (Integer)rightNumber;
					var result = _componentType == ComponentType.Multiply
						? leftFraction * rightInteger
						: _componentType == ComponentType.Divide
							? leftFraction / rightInteger
							: leftFraction ^ rightInteger;
					return new SingleFactorComponent(new ExpressionFactor(new SingleComponentExpression(result)));
				}
				else
				{
					var rightDouble = (Float)rightNumber;
					var result = _componentType == ComponentType.Multiply
						? leftFraction * rightDouble
						: _componentType == ComponentType.Divide
							? leftFraction / rightDouble
							: leftFraction ^ rightDouble;
					return new SingleFactorComponent(new NumericFactor(result));
				}
			}

			return new DualFactorComponent(left, right, _componentType);
		}
	}
}
