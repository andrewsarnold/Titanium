using System.Collections.Generic;
using System.Linq;
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
		internal readonly Component LeftComponent;
		internal readonly Component RightComponent;
		private readonly bool _isAdd;

		internal DualComponentExpression(Component leftComponent, Component rightComponent, bool isAdd)
		{
			_isAdd = isAdd;
			LeftComponent = leftComponent;
			RightComponent = rightComponent;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", LeftComponent, _isAdd ? "+" : "-", RightComponent);
		}

		internal override Expression Evaluate(bool expand = false)
		{
			var left = LeftComponent.Evaluate(expand);
			var right = RightComponent.Evaluate(expand);

			if (left.Equals(right))
			{
				if (!_isAdd)
				{
					return Expressionizer.ToExpression(new NumericFactor(new Integer(0)));
				}

				return Expressionizer.ToExpression(new ComponentList(new List<ComponentListFactor>
				{
					new ComponentListFactor(new NumericFactor(new Integer(2))),
					new ComponentListFactor(Factorizer.ToFactor(LeftComponent))
				})).Evaluate(expand);
			}
			
			Expression returnValue;
			if (CombineComponentsSharingAFactor(left, right, out returnValue) ||
				CombineNaturalLogs(left, right, _isAdd, out returnValue) ||
				CombineNaturalLogs(LeftComponent, RightComponent, _isAdd, out returnValue))
			{
				return returnValue;
			}

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
					return new DualComponentExpression(newRight, Componentizer.ToComponent(left), true).Evaluate(expand);
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

		public static Expression operator *(DualComponentExpression left, DualComponentExpression right)
		{
			// (a+b)*(c+d) == a*c + a*d + b*c + b*d
			var first = new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(Factorizer.ToFactor(left.LeftComponent)),
				new ComponentListFactor(Factorizer.ToFactor(right.LeftComponent))
			});
			var second = new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(Factorizer.ToFactor(left.LeftComponent)),
				new ComponentListFactor(Factorizer.ToFactor(right.RightComponent))
			});
			var third = new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(Factorizer.ToFactor(left.RightComponent)),
				new ComponentListFactor(Factorizer.ToFactor(right.LeftComponent))
			});
			var fourth = new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(Factorizer.ToFactor(left.RightComponent)),
				new ComponentListFactor(Factorizer.ToFactor(right.RightComponent))
			});
			
			var thirdPlusFourth = new DualComponentExpression(Componentizer.ToComponent(third), Componentizer.ToComponent(fourth), true);
			var secondPlusThirdPlusFourth = new DualComponentExpression(Componentizer.ToComponent(second), Componentizer.ToComponent(thirdPlusFourth), true);
			var firstPlusSecondPlusThirdPlusFourth = new DualComponentExpression(Componentizer.ToComponent(first), Componentizer.ToComponent(secondPlusThirdPlusFourth), true);
			return firstPlusSecondPlusThirdPlusFourth.Evaluate();
		}

		private static bool CombineComponentsSharingAFactor(Evaluatable leftExpression, Evaluatable rightExpression, out Expression returnValue)
		{
			ComponentList left;
			ComponentList right;

			if (Common.IsComponentList(leftExpression, out left) && Common.IsComponentList(rightExpression, out right))
			{
				// Can reduce if all alphabetic factors match and the only other factors are numeric
				var leftAlphas = left.Factors.Where(f => f.Factor is AlphabeticFactor).ToList();
				var rightAlphas = right.Factors.Where(f => f.Factor is AlphabeticFactor).ToList();
				var bothAlphas = leftAlphas.Select(a => a.Factor).Union(rightAlphas.Select(a => a.Factor)).Select(f => f.ToString()).Distinct().ToList();
				if (bothAlphas.Count == leftAlphas.Count)
				{
					if (left.Factors.Where(f => !(f.Factor is AlphabeticFactor)).All(f => f.Factor is NumericFactor) &&
					    right.Factors.Where(f => !(f.Factor is AlphabeticFactor)).All(f => f.Factor is NumericFactor))
					{
						var numericFactors = left.Factors.Where(f => f.Factor is NumericFactor).Union(right.Factors.Where(f => f.Factor is NumericFactor));
						var finalValue = Expressionizer.ToExpression(new NumericFactor(new Integer(0)));
						foreach (var numericFactor in numericFactors)
						{
							finalValue = new DualComponentExpression(Componentizer.ToComponent(finalValue), Componentizer.ToComponent(numericFactor.Factor), numericFactor.IsInNumerator).Evaluate();
						}

						var combined = new ComponentList(new List<ComponentListFactor>
						{
							new ComponentListFactor(Factorizer.ToFactor(finalValue))
						}.Union(leftAlphas).ToList());
						returnValue = combined.Evaluate();
						return true;
					}
				}
			}

			returnValue = null;
			return false;
		}

		private static bool CombineNaturalLogs(Evaluatable leftExpression, Evaluatable rightExpression, bool isAdd, out Expression singleComponentExpression)
		{
			FunctionComponent leftFunction;
			FunctionComponent rightFunction;

			if (Common.IsFunction(leftExpression, out leftFunction) && Common.IsFunction(rightExpression, out rightFunction))
			{
				if (leftFunction.Function.Name == "ln" && rightFunction.Function.Name == "ln")
				{
					var operand = Expressionizer.ToExpression(new DualFactorComponent(Factorizer.ToFactor(leftFunction.Operands[0]), Factorizer.ToFactor(rightFunction.Operands[0]), isAdd));
					{
						singleComponentExpression = new SingleComponentExpression(new FunctionComponent("ln", new List<Expression> { operand.Evaluate() }));
						return true;
					}
				}
			}
			singleComponentExpression = null;
			return false;
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
				return LeftComponent.Equals(dce.LeftComponent) &&
				       RightComponent.Equals(dce.RightComponent) &&
				       _isAdd == dce._isAdd;
			}

			return false;
		}
	}
}
