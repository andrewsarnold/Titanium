using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;
using Titanium.Core.Functions.Implementations;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Expressions
{
	internal class DualComponentExpression : Expression
	{
		private readonly Component _leftComponent;
		private readonly Component _rightComponent;
		private readonly bool _isAdd;

		internal DualComponentExpression(Component leftComponent, Component rightComponent, bool isAdd)
		{
			_isAdd = isAdd;
			_leftComponent = leftComponent;
			_rightComponent = rightComponent;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", _leftComponent, _isAdd ? "+" : "-", _rightComponent);
		}

		internal override Expression Evaluate()
		{
			if (_leftComponent.Equals(_rightComponent))
			{
				if (!_isAdd)
				{
					return Expressionizer.ToExpression(new NumericFactor(new Integer(0)));
				}

				return Expressionizer.ToExpression(new ComponentList(new List<ComponentListFactor>
				{
					new ComponentListFactor(new NumericFactor(new Integer(2))),
					new ComponentListFactor(Factorizer.ToFactor(_leftComponent))
				})).Evaluate();
			}

			// Combine natural log functions
			// TODO: Move somewhere else
			FunctionComponent leftFunction;
			FunctionComponent rightFunction;

			if (Common.IsFunction(_leftComponent, out leftFunction) && Common.IsFunction(_rightComponent, out rightFunction))
			{
				if (leftFunction.Function.Name == "ln" && rightFunction.Function.Name == "ln")
				{
					var operand = Expressionizer.ToExpression(new DualFactorComponent(Factorizer.ToFactor(leftFunction.Operands[0]), Factorizer.ToFactor(rightFunction.Operands[0]), _isAdd));
					return new SingleComponentExpression(new FunctionComponent("ln", new List<Expression> { operand.Evaluate() }));
				}
			}

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

			if (Common.IsNumber(left, out leftNumber) && leftNumber.IsZero)
			{
				return _isAdd
					? Expressionizer.ToExpression(right)
					: new Negate().Evaluate(Expressionizer.ToExpression(right));
			}

			if (Common.IsNumber(right, out rightNumber) && rightNumber.IsZero)
			{
				return Expressionizer.ToExpression(right);
			}

			if (left.CompareTo(right) == 1)
			{
				// Swap placement
				if (!_isAdd)
				{
					var newRight = Componentizer.ToComponent(new FunctionComponent(new Negate(), new List<Expression> { right }));
					return new DualComponentExpression(newRight, Componentizer.ToComponent(left), true).Evaluate();
				}

				var newLeft = Componentizer.ToComponent(left);
				if (newLeft is FunctionComponent)
				{
					if (((FunctionComponent) newLeft).Function is Negate)
					{
						return new DualComponentExpression(Componentizer.ToComponent(right), Componentizer.ToComponent(((FunctionComponent) newLeft).Operands[0]), false);
					}
				}
				return new DualComponentExpression(Componentizer.ToComponent(right), newLeft, true);
			}

			return new DualComponentExpression(Componentizer.ToComponent(left), Componentizer.ToComponent(right), _isAdd);
		}

		public override int CompareTo(object obj)
		{
			var other = obj as DualFactorComponent;
			if (other != null)
			{

			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var dce = other as DualComponentExpression;
			if (dce != null)
			{
				return _leftComponent.Equals(dce._leftComponent) &&
				       _rightComponent.Equals(dce._rightComponent) &&
				       _isAdd == dce._isAdd;
			}

			return false;
		}
	}
}
