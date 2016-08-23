using System;
using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Functions.Implementations;
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

		internal ComponentList(List<ComponentListFactor> factors)
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

			if (component is IntegerFraction)
			{
				var frac = (IntegerFraction)component;
				return new List<ComponentListFactor>
				{
					new ComponentListFactor(new NumericFactor(new Integer(frac.Numerator))),
					new ComponentListFactor(new NumericFactor(new Integer(frac.Denominator)), false)
				};
			}

			if (component is ComponentList)
			{
				return ((ComponentList)component).Factors.Select(f => new ComponentListFactor(f.Factor, isMultiply == f.IsInNumerator)).ToList();
			}

			return new List<ComponentListFactor> { new ComponentListFactor(Factorizer.ToFactor(component), isMultiply) };
		}

		internal override Expression Evaluate()
		{
			return Reduce(Factors);
		}

		public override int CompareTo(object obj)
		{
			var other = obj as ComponentList;
			if (other != null)
			{
				if (Factors.Count < other.Factors.Count) return -1;
				if (Factors.Count > other.Factors.Count) return 1;
				return Factors.Select((t, x) => t.CompareTo(other.Factors[x])).FirstOrDefault(compResult => compResult != 0);
			}

			var sfc = obj as SingleFactorComponent;
			if (sfc != null)
			{
				return -1;
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var componentList = other as ComponentList;
			if (componentList != null)
			{
				if (Factors.Count < componentList.Factors.Count || Factors.Count > componentList.Factors.Count) return false;
				return !Factors.Where((t, i) => !t.Equals(componentList.Factors[i])).Any();
			}

			return false;
		}

		private static Expression Reduce(IEnumerable<ComponentListFactor> factors)
		{
			var evaluated = factors.Select(f => new ComponentListFactor(Factorizer.ToFactor(f.Evaluate()), f.IsInNumerator)).ToList();
			RemoveRedundantOnes(evaluated, true);

			// If any components are a two-factor component, split it out and put the factors into the main list
			var reducedEvaluated = new List<ComponentListFactor>();
			foreach (var componentListFactor in evaluated)
			{
				var asComponent = Componentizer.ToComponent(componentListFactor.Factor);
				if (asComponent is ComponentList)
				{
					var list = (ComponentList)asComponent;
					foreach (var listFactor in list.Factors)
					{
						listFactor.IsInNumerator = componentListFactor.IsInNumerator == listFactor.IsInNumerator;
					}
					reducedEvaluated.AddRange(list.Factors);
				}
				else
				{
					reducedEvaluated.Add(componentListFactor);
				}
			}

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

			while (reducedEvaluated.Any())
			{
				var reduced = false;
				for (var i = 1; i < reducedEvaluated.Count; i++)
				{
					Expression e;
					if (CanReduce(reducedEvaluated[0], reducedEvaluated[i], out e))
					{
						output.Add(new ComponentListFactor(Factorizer.ToFactor(e)));
						reducedEvaluated.RemoveAt(i);
						reducedEvaluated.RemoveAt(0);
						i = 1;

						reduced = true;
						reducedAny = true;
					}
				}

				if (!reduced && reducedEvaluated.Any())
				{
					output.Add(reducedEvaluated[0]);
					reducedEvaluated.RemoveAt(0);
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

			RemoveRedundantOnes(output, false);
			return Reduce(output);
		}

		private static void RemoveRedundantOnes(List<ComponentListFactor> output, bool integerOnly)
		{
			// Remove redundant 1s in numerator
			if (output.Count(o => o.IsInNumerator) > 1)
			{
				output.RemoveAll(o =>
					o.IsInNumerator &&
					o.Factor is NumericFactor &&
					(!integerOnly || ((NumericFactor)o.Factor).Number is Integer) &&
					((NumericFactor)o.Factor).Number.IsOne
				);
			}

			// Remove redundant 1s in denominator
			output.RemoveAll(o =>
				!o.IsInNumerator &&
				o.Factor is NumericFactor &&
				(!integerOnly || ((NumericFactor)o.Factor).Number is Integer) &&
				((NumericFactor)o.Factor).Number.IsOne
			);
		}

		private static bool CanReduce(ComponentListFactor leftFactor, ComponentListFactor rightFactor, out Expression expression)
		{
			if (ReduceMultipliedAlphabeticFactors(leftFactor, rightFactor, out expression))
			{
				return true;
			}

			var left = Expressionizer.ToExpression(leftFactor.Factor);
			var right = Expressionizer.ToExpression(rightFactor.Factor);

			if (left.Equals(right) && leftFactor.IsInNumerator == rightFactor.IsInNumerator)
			{
				expression = new ComponentList(new List<ComponentListFactor>
				{
					new ComponentListFactor(new NumericFactor(new Integer(1))),
					new ComponentListFactor(Factorizer.ToFactor(new Exponent().Evaluate(left, NumberToExpression(new Integer(2)))), leftFactor.IsInNumerator)
				}).Evaluate();
				return true;
			}

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

			if (ReduceFunctionsWithSameOperands(leftFactor, rightFactor, out expression, left, right))
			{
				return true;
			}

			if (ReduceExponents(out expression, left, right))
			{
				return true;
			}

			expression = null;
			return false;
		}

		private static bool ReduceMultipliedAlphabeticFactors(ComponentListFactor leftFactor, ComponentListFactor rightFactor, out Expression expression)
		{
			// If any alphabetic factors are identical, convert to exponent
			if (leftFactor.Factor is AlphabeticFactor && rightFactor.Factor is AlphabeticFactor)
			{
				var leftAlpha = (AlphabeticFactor) leftFactor.Factor;
				var rightAlpha = (AlphabeticFactor) rightFactor.Factor;

				if (leftAlpha.Value.Equals(rightAlpha.Value))
				{
					var shouldPower = leftFactor.IsInNumerator && rightFactor.IsInNumerator;
					if (shouldPower)
					{
						expression = new SingleComponentExpression(new FunctionComponent("^", new List<Expression> { Expressionizer.ToExpression(leftAlpha), NumberToExpression(new Integer(2)) }));
						return true;
					}

					expression = NumberToExpression(new Integer(1));
					return true;
				}
			}

			expression = null;
			return false;
		}

		private static bool ReduceFunctionsWithSameOperands(ComponentListFactor leftFactor, ComponentListFactor rightFactor, out Expression expression, Expression left, Expression right)
		{
			FunctionComponent leftFunction;
			FunctionComponent rightFunction;
			// If there are two functions with the same operands that should cancel out, cancel them out
			if (Common.IsFunction(left, out leftFunction) && Common.IsFunction(right, out rightFunction))
			{
				if (leftFunction.Function.Name == rightFunction.Function.Name &&
				    leftFunction.Operands.Count == rightFunction.Operands.Count)
				{
					for (var i = 0; i < leftFunction.Operands.Count; i++)
					{
						var leftOperand = leftFunction.Operands[i];
						var rightOperand = rightFunction.Operands[i];

						if (!leftOperand.ToString().Equals(rightOperand.ToString()))
						{
							break;
						}

						if (leftFactor.IsInNumerator != rightFactor.IsInNumerator)
						{
							expression = NumberToExpression(new Integer(1));
							return true;
						}
					}
				}
			}

			expression = null;
			return false;
		}

		private static bool ReduceExponents(out Expression expression, Expression left, Expression right)
		{
			FunctionComponent leftFunction;
			FunctionComponent rightFunction;
			// If there is an exponent times another instance of that base, combine them
			// e.g. x^2 * x = x^3
			if (Common.IsFunction(left, out leftFunction) && leftFunction.Function is Exponent)
			{
				var baseExpression = leftFunction.Operands[0];
				var power = Factorizer.ToFactor(leftFunction.Operands[1]);
				if (power is NumericFactor && baseExpression.Equals(right))
				{
					expression = Expressionizer.ToExpression(new FunctionComponent(new Exponent(), new List<Expression>
					{
						baseExpression,
						Expressionizer.ToExpression(new NumericFactor(((NumericFactor) power).Number + new Integer(1)))
					}));
					return true;
				}
			}
			if (Common.IsFunction(right, out rightFunction) && rightFunction.Function is Exponent)
			{
				var baseExpression = rightFunction.Operands[0];
				var power = Factorizer.ToFactor(rightFunction.Operands[1]);
				if (power is NumericFactor && baseExpression.Equals(left))
				{
					expression = Expressionizer.ToExpression(new FunctionComponent(new Exponent(), new List<Expression>
					{
						baseExpression,
						Expressionizer.ToExpression(new NumericFactor(((NumericFactor) power).Number + new Integer(1)))
					}));
					return true;
				}
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
				: (result is Number
					? Expressionizer.ToExpression(Componentizer.ToComponent(new NumericFactor((Number)result)))
					: Expressionizer.ToExpression((SingleFactorComponent)result));
		}

		public override string ToString()
		{
			var numerators = Factors.Where(f => f.IsInNumerator).OrderBy(f => f).ToList();
			var denominators = Factors.Where(f => !f.IsInNumerator).OrderBy(f => f).ToList();

			var numString = string.Join("*", numerators.Select(f => f.Factor));
			var denomString = string.Join("*", denominators.Select(f => f.Factor));

			if (numerators.Any() && denominators.Any())
			{
				return string.Format("{0}/{1}",
					numString,
					denomString.Contains("*") || denomString.Contains(".") ? string.Format("({0})", denomString) : denomString);
			}

			if (numerators.Any())
			{
				return numString;
			}

			if (denominators.Any())
			{
				return string.Format("1/{0}", denomString.Contains("*") ? string.Format("({0})", denomString) : denomString);
			}

			throw new Exception("No components");
		}
	}
}
