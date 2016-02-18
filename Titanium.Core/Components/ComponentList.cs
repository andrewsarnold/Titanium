using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	internal class ComponentList : Component
	{
		internal readonly List<ComponentListFactor> Factors;

		internal ComponentList(Component component)
		{
			Factors = GetComponents(component);
		}

		private ComponentList(List<ComponentListFactor> factors)
		{
			Factors = factors;
		}

		private static List<ComponentListFactor> GetComponents(Component component)
		{
			if (component is DualFactorComponent)
			{
				var dfc = (DualFactorComponent)component;
				var leftComponent = Componentizer.ToComponent(dfc.LeftFactor);
				var rightComponent = Componentizer.ToComponent(dfc.RightFactor);

				var leftList = GetComponents(leftComponent);
				var rightList = GetComponents(rightComponent);

				return leftList.Union(rightList).ToList();
			}

			return new List<ComponentListFactor> { new ComponentListFactor(Factorizer.ToFactor(component)) };
		}

		internal override Expression Evaluate()
		{
			var evaluated = Factors.Select(f => new ComponentListFactor(Factorizer.ToFactor(f.Evaluate()))).ToList();

			// Loop through all combinations of two factors
			// If two factors can reduce, add to output list and remove from input list
			// Continue until no reductions are possible

			// Start with first factor
			// Loop through other factors
			// If two can be reduced:
			// - Append result to output list
			// - Remove two from input list
			// Otherwise:
			// - Append factor to output list
			// - Remove it from input list

			var output = new List<Expression>();

			while (evaluated.Any())
			{
				var matched = false;
				for (var i = 1; i < evaluated.Count; i++)
				{
					Expression e;
					if (CanReduce(evaluated[0], evaluated[i], out e))
					{
						output.Add(e);
						evaluated.RemoveAt(i);
						evaluated.RemoveAt(0);
						i = 1;

						matched = true;
					}
				}

				if (!matched && evaluated.Any())
				{
					output.Add(Expressionizer.ToExpression(evaluated[0].Factor));
					evaluated.RemoveAt(0);
				}
			}

			// If no factors could be reduced, return as expression
			// Otherwise, repeat the whole process on the new list

			return Expressionizer.ToExpression(new ComponentList(output.Select(o => new ComponentListFactor(Factorizer.ToFactor(o))).ToList()));
		}

		private bool CanReduce(Evaluatable leftFactor, Evaluatable rightFactor, out Expression expression)
		{
			var left = leftFactor.Evaluate();
			var right = rightFactor.Evaluate();

			Number leftNumber;
			Number rightNumber;

			if ((Common.IsConstant(left, out leftNumber) && Common.IsFloat(right, out rightNumber)) ||
				(Common.IsFloat(left, out leftNumber) && Common.IsConstant(right, out rightNumber)) ||
				(Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber)))
			{
				expression = Evaluate(leftNumber, rightNumber, true);
				return true;
			}

			ExpressionList leftList;
			ExpressionList rightList;

			if (Common.IsList(left, out leftList))
			{
				expression = Evaluate(leftList, right, true);
				return true;
			}

			if (Common.IsList(right, out rightList))
			{
				expression = Evaluate(rightList, left, true);
				return true;
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsNumber(left, out leftNumber) && Common.IsIntegerFraction(right, out rightFraction))
			{
				expression = Expressionizer.ToExpression(true
					? leftNumber * rightFraction
					: leftNumber / rightFraction);
				return true;
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				expression = Expressionizer.ToExpression(true
					? leftFraction * rightNumber
					: leftFraction / rightNumber);
				return true;
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				expression = Expressionizer.ToExpression(true
					? leftFraction * rightFraction
					: leftFraction / rightFraction);
				return true;
			}

			expression = null;
			return false;
		}

		private static Expression Evaluate(ExpressionList leftNumber, Evaluatable right, bool isMultiply)
		{
			return Expressionizer.ToExpression(new ExpressionList(leftNumber.Expressions.Select(e => new DualFactorComponent(Factorizer.ToFactor(e), Factorizer.ToFactor(right), isMultiply).Evaluate()).ToList()));
		}

		private Expression Evaluate(Number leftNumber, Number rightNumber, bool isMultiply)
		{
			if (isMultiply)
			{
				return Expressionizer.ToExpression(new NumericFactor(leftNumber * rightNumber));
			}

			var result = leftNumber / rightNumber;
			return result is IntegerFraction
				? Expressionizer.ToExpression((IntegerFraction)result)
				: Expressionizer.ToExpression(new SingleFactorComponent(new NumericFactor((Number)result)));
		}

		public override string ToString()
		{
			return string.Join("*", Factors.Select(f => f.Factor));
		}
	}
}
