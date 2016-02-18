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

		private static List<ComponentListFactor> GetComponents(Component component, bool isMultiply = true)
		{
			if (component is DualFactorComponent)
			{
				var dfc = (DualFactorComponent)component;
				var leftComponent = Componentizer.ToComponent(dfc.LeftFactor);
				var rightComponent = Componentizer.ToComponent(dfc.RightFactor);

				var leftList = GetComponents(leftComponent, isMultiply);
				var rightList = GetComponents(rightComponent, isMultiply == dfc.IsMultiply);

				return leftList.Union(rightList).ToList();
			}

			return new List<ComponentListFactor> { new ComponentListFactor(Factorizer.ToFactor(component), isMultiply) };
		}

		internal override Expression Evaluate()
		{
			return Reduce(Factors);
		}

		private static Expression Reduce(IEnumerable<ComponentListFactor> factors)
		{
			var evaluated = factors.Select(f => new ComponentListFactor(Factorizer.ToFactor(f.Evaluate()), f.IsInNumerator)).ToList();

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

			var output = new List<ComponentListFactor>();
			var reducedAny = false;

			while (evaluated.Any())
			{
				var reduced = false;
				for (var i = 1; i < evaluated.Count; i++)
				{
					Expression e;
					if (CanReduce(evaluated[0], evaluated[i], out e))
					{
						output.Add(new ComponentListFactor(Factorizer.ToFactor(e)));
						evaluated.RemoveAt(i);
						evaluated.RemoveAt(0);
						i = 1;

						reduced = true;
						reducedAny = true;
					}
				}

				if (!reduced && evaluated.Any())
				{
					output.Add(evaluated[0]);
					evaluated.RemoveAt(0);
				}
			}

			// If no factors could be reduced, return as expression
			// Otherwise, repeat the whole process on the new list

			if (!reducedAny)
			{
				return output.Count == 1
					? Expressionizer.ToExpression(output[0])
					: Expressionizer.ToExpression(new ComponentList(output));
			}
			
			return Reduce(output);
		}

		private static bool CanReduce(ComponentListFactor leftFactor, ComponentListFactor rightFactor, out Expression expression)
		{
			var left = leftFactor.Evaluate();
			var right = rightFactor.Evaluate();

			Number leftNumber;
			Number rightNumber;

			if ((Common.IsConstant(left, out leftNumber) && Common.IsFloat(right, out rightNumber)) ||
				(Common.IsFloat(left, out leftNumber) && Common.IsConstant(right, out rightNumber)) ||
				(Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber)))
			{
				expression = Evaluate(leftNumber, rightNumber, leftFactor.IsInNumerator, rightFactor.IsInNumerator);
				return true;
			}

			ExpressionList leftList;
			ExpressionList rightList;

			if (Common.IsList(left, out leftList))
			{
				expression = Evaluate(leftList, right, leftFactor.IsInNumerator == rightFactor.IsInNumerator);
				return true;
			}

			if (Common.IsList(right, out rightList))
			{
				expression = Evaluate(rightList, left, leftFactor.IsInNumerator == rightFactor.IsInNumerator);
				return true;
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsNumber(left, out leftNumber) && Common.IsIntegerFraction(right, out rightFraction))
			{
				expression = Expressionizer.ToExpression(Evaluate(leftNumber, rightFraction, leftFactor.IsInNumerator, rightFactor.IsInNumerator));
				return true;
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				expression = Expressionizer.ToExpression(Evaluate(leftFraction, rightNumber, leftFactor.IsInNumerator, rightFactor.IsInNumerator));
				return true;
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				expression = Expressionizer.ToExpression(Evaluate(leftFraction, rightFraction, leftFactor.IsInNumerator, rightFactor.IsInNumerator));
				return true;
			}

			expression = null;
			return false;
		}

		private static Expression Evaluate(ExpressionList leftNumber, Evaluatable right, bool isMultiply)
		{
			return Expressionizer.ToExpression(new ExpressionList(leftNumber.Expressions.Select(e => new DualFactorComponent(Factorizer.ToFactor(e), Factorizer.ToFactor(right), isMultiply).Evaluate()).ToList()));
		}

		private static Expression Evaluate(Number leftNumber, Number rightNumber, bool leftIsNumerator, bool rightIsNumerator)
		{
			if (leftIsNumerator && rightIsNumerator)
			{
				return Expressionizer.ToExpression(new NumericFactor(leftNumber * rightNumber));
			}

			if (leftIsNumerator)
			{
				return NumberToExpression(leftNumber / rightNumber);
			}

			if (rightIsNumerator)
			{
				return NumberToExpression(rightNumber / leftNumber);
			}

			return NumberToExpression(new Integer(1) / (leftNumber * rightNumber));
		}

		private static Expression Evaluate(Number leftNumber, IntegerFraction rightFraction, bool leftIsNumerator, bool rightIsNumerator)
		{
			if (leftIsNumerator && rightIsNumerator)
			{
				return Expressionizer.ToExpression(leftNumber * rightFraction);
			}

			if (leftIsNumerator)
			{
				return NumberToExpression(leftNumber / rightFraction);
			}

			if (rightIsNumerator)
			{
				return NumberToExpression(rightFraction / leftNumber);
			}

			return NumberToExpression(rightFraction.Inverse / leftNumber);
		}

		private static Expression Evaluate(IntegerFraction leftFraction, Number rightNumber, bool leftIsNumerator, bool rightIsNumerator)
		{
			if (leftIsNumerator && rightIsNumerator)
			{
				return Expressionizer.ToExpression(leftFraction * rightNumber);
			}

			if (leftIsNumerator)
			{
				return NumberToExpression(leftFraction / rightNumber);
			}

			if (rightIsNumerator)
			{
				return NumberToExpression(rightNumber / leftFraction);
			}

			return NumberToExpression(leftFraction.Inverse / rightNumber);
		}

		private static Expression Evaluate(IntegerFraction leftFraction, IntegerFraction rightFraction, bool leftIsNumerator, bool rightIsNumerator)
		{
			if (leftIsNumerator && rightIsNumerator)
			{
				return Expressionizer.ToExpression(leftFraction * rightFraction);
			}

			if (leftIsNumerator)
			{
				return NumberToExpression(leftFraction / rightFraction);
			}

			if (rightIsNumerator)
			{
				return NumberToExpression(rightFraction / leftFraction);
			}

			return NumberToExpression(leftFraction.Inverse * rightFraction.Inverse);
		}

		private static Expression NumberToExpression(object result)
		{
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
