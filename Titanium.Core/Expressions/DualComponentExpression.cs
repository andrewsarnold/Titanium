using Titanium.Core.Components;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Expressions
{
	internal class DualComponentExpression : Expression
	{
		private readonly Component _leftComponent;
		private readonly Component _rightComponent;
		private readonly bool _isAdd;

		public DualComponentExpression(Component leftComponent, Component rightComponent, bool isAdd)
		{
			_isAdd = isAdd;
			_leftComponent = leftComponent;
			_rightComponent = rightComponent;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", _leftComponent, _isAdd ? "+" : "-", _rightComponent);
		}

		public override Expression Evaluate()
		{
			var left = _leftComponent.Evaluate();
			var right = _rightComponent.Evaluate();

			Number leftNumber;
			Number rightNumber;

			if (Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber))
			{
				return Expressionizer.ToExpression(new NumericFactor(_isAdd
					? leftNumber + rightNumber
					: leftNumber - rightNumber));
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				return new SingleComponentExpression(_isAdd
					? leftFraction + rightFraction
					: leftFraction - rightFraction);
			}

			if (leftFraction != null && Common.IsNumber(right, out rightNumber))
			{
				return new SingleComponentExpression(_isAdd
					? leftFraction + rightNumber
					: leftFraction - rightNumber);
			}

			if (leftNumber != null && Common.IsIntegerFraction(right, out rightFraction))
			{
				return new SingleComponentExpression(_isAdd
					? leftNumber + rightFraction
					: leftNumber - rightFraction);
			}

			return new DualComponentExpression(Componentizer.ToComponent(left), Componentizer.ToComponent(right), _isAdd);
		}
	}
}
