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
	internal class ExpressionList : Expression
	{
		internal readonly List<ExpressionListComponent> Components;

		public ExpressionList(Expression expression)
		{
			Components = GetComponents(expression);
		}

		internal ExpressionList(List<ExpressionListComponent> components)
		{
			Components = components;
		}

		private List<ExpressionListComponent> GetComponents(Expression expression, bool isAdd = true)
		{
			if (expression is DualComponentExpression)
			{
				var dce = (DualComponentExpression)expression;
				var leftComponent = Expressionizer.ToExpression(dce.LeftComponent);
				var rightComponent = Expressionizer.ToExpression(dce.RightComponent);

				var leftList = GetComponents(leftComponent, isAdd);
				var rightList = GetComponents(rightComponent, isAdd == dce.IsAdd);

				return leftList.Union(rightList).ToList();
			}

			if (expression is ExpressionList)
			{
				return ((ExpressionList)expression).Components.Select(f => new ExpressionListComponent(f.Component, isAdd == f.IsAdd)).ToList();
			}

			return new List<ExpressionListComponent> { new ExpressionListComponent(Componentizer.ToComponent(expression), isAdd) };
		}

		internal override Expression Evaluate(bool expand = false)
		{
			return Reduce(Components, expand);
		}

		private static Expression Reduce(IEnumerable<ExpressionListComponent> components, bool expand)
		{
			var evaluated = components.Select(c => new ExpressionListComponent(Componentizer.ToComponent(c.Evaluate(expand)), c.IsAdd)).ToList();
			RemoveRedundantZeros(evaluated, true);

			// If any expressions are a two-component expression, split it out and put the components into the main list
			var reducedEvaluated = new List<ExpressionListComponent>();
			foreach (var expressionListComponent in evaluated)
			{
				var asExpression = Expressionizer.ToExpression(expressionListComponent.Component);
				if (asExpression is ExpressionList)
				{
					var list = (ExpressionList)asExpression;
					foreach (var listComponent in list.Components)
					{
						listComponent.IsAdd = expressionListComponent.IsAdd == listComponent.IsAdd;
					}
					reducedEvaluated.AddRange(list.Components);
				}
				else
				{
					reducedEvaluated.Add(expressionListComponent);
				}
			}

			// Loop through all combinations of two components
			// If two components can reduce, add to output list and remove from input list
			// Continue until no reductions are possible

			// Start with first component
			// Loop through other components
			// If two can be reduced:
			// - Append result to output list
			// - Remove two from input list
			// Otherwise:
			// - Append component to output list
			// - Remove it from input list

			var output = new List<ExpressionListComponent>();
			var reducedAny = false;

			while (reducedEvaluated.Any())
			{
				var reduced = false;
				for (var i = 1; i < reducedEvaluated.Count; i++)
				{
					Expression e;
					if (CanReduce(reducedEvaluated[0], reducedEvaluated[i], reducedEvaluated[i].IsAdd, expand, out e))
					{
						output.Add(new ExpressionListComponent(Componentizer.ToComponent(e)));
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
				//output = output.OrderBy(o => o.Component).ToList();
				return output.Count == 1
					? Expressionizer.ToExpression(output[0])
					: new ExpressionList(output);
			}

			RemoveRedundantZeros(output, false);
			return Reduce(output, expand);
		}

		private static void RemoveRedundantZeros(List<ExpressionListComponent> output, bool integerOnly)
		{
			output.RemoveAll(o =>
			{
				var factor = Factorizer.ToFactor(o.Component);
				return factor is NumericFactor &&
				       (!integerOnly || ((NumericFactor)factor).Number is Integer) &&
				       ((NumericFactor)factor).Number.IsZero;
			});
		}

		private static bool CanReduce(ExpressionListComponent leftComponent, ExpressionListComponent rightComponent, bool isAdd, bool expand, out Expression expression)
		{
			var left = leftComponent.Evaluate(expand);
			var right = rightComponent.Evaluate(expand);

			if (left.Equals(right))
			{
				if (!isAdd)
				{
					expression = Expressionizer.ToExpression(new NumericFactor(new Integer(0)));
					return true;
				}

				expression = Expressionizer.ToExpression(new ComponentList(new List<ComponentListFactor>
				{
					new ComponentListFactor(new NumericFactor(new Integer(2))),
					new ComponentListFactor(Factorizer.ToFactor(leftComponent))
				})).Evaluate(expand);
				return true;
			}

			Expression returnValue;
			if (CombineComponentsSharingAFactor(left, right, out returnValue) ||
				CombineNaturalLogs(left, right, isAdd, out returnValue) ||
				CombineNaturalLogs(leftComponent, rightComponent, isAdd, out returnValue))
			{
				expression = returnValue;
				return true;
			}

			Number leftNumber;
			Number rightNumber;

			if (Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber))
			{
				leftNumber = leftComponent.IsAdd ? leftNumber : leftNumber.Negate();
				rightNumber = rightComponent.IsAdd ? rightNumber : rightNumber.Negate();
				expression = Expressionizer.ToExpression(new NumericFactor(leftNumber + rightNumber));
				return true;
			}

			IntegerFraction leftFraction;
			IntegerFraction rightFraction;

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				expression = new SingleComponentExpression(isAdd
					? leftFraction + rightFraction
					: leftFraction - rightFraction);
				return true;
			}

			if (leftFraction != null && Common.IsNumber(right, out rightNumber))
			{
				expression = new SingleComponentExpression(isAdd
					? leftFraction + rightNumber
					: leftFraction - rightNumber);
				return true;
			}

			if (leftNumber != null && Common.IsIntegerFraction(right, out rightFraction))
			{
				expression = new SingleComponentExpression(isAdd
					? leftNumber + rightFraction
					: leftNumber - rightFraction);
				return true;
			}

			if (Common.IsNumber(left, out leftNumber) && leftNumber.IsZero)
			{
				expression = isAdd
					? Expressionizer.ToExpression(right)
					: new Negate().Evaluate(Expressionizer.ToExpression(right));
				return true;
			}

			if (Common.IsNumber(right, out rightNumber) && rightNumber.IsZero)
			{
				expression = Expressionizer.ToExpression(right);
				return true;
			}

			expression = null;
			return false;
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

		public static Expression operator *(ExpressionList left, ExpressionList right)
		{
			var results = new List<Expression>();
			foreach (var leftComponent in left.Components)
			{
				foreach (var rightComponent in right.Components)
				{
					results.Add(new ComponentList(new List<ComponentListFactor>
					{
						new ComponentListFactor(Factorizer.ToFactor(leftComponent)),
						new ComponentListFactor(Factorizer.ToFactor(rightComponent))
					}).Evaluate());
				}
			}

			return new ExpressionList(results.Select(r => new ExpressionListComponent(Componentizer.ToComponent(r.Evaluate()))).ToList())
				.Evaluate();
		}

		public override int CompareTo(object obj)
		{
			var other = obj as ExpressionList;
			if (other != null)
			{
				if (Components.Count < other.Components.Count) return -1;
				if (Components.Count > other.Components.Count) return 1;
				return Components.Select((t, x) => t.CompareTo(other.Components[x])).FirstOrDefault(compResult => compResult != 0);
			}

			var sfc = obj as SingleComponentExpression;
			if (sfc != null)
			{
				return -1;
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var expressionList = other as ExpressionList;
			if (expressionList != null)
			{
				if (Components.Count < expressionList.Components.Count || Components.Count > expressionList.Components.Count) return false;
				return !Components.Where((t, i) => !t.Equals(expressionList.Components[i])).Any();
			}

			return false;
		}

		public override string ToString()
		{
			if (Components.Count == 0)
			{
				return "0";
			}

			var adds = Components.Where(c => c.IsAdd).Select(c => c.Component).OrderBy(c => c);
			var subtracts = Components.Where(c => !c.IsAdd).Select(c => c.Component).OrderBy(c => c);
			return "" + string.Join("+", adds) + (subtracts.Any() ? "-" : "") + string.Join("-", subtracts);
		}
	}
}
