using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class Exponent : Function
	{
		internal Exponent()
			: base("^", 2, FixType.MidFix)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			var left = parameters[0].Evaluate();
			var right = parameters[1].Evaluate();

			Number leftNumber;
			Number rightNumber;

			if ((Common.IsConstant(left, out leftNumber) && Common.IsFloat(right, out rightNumber)) ||
				(Common.IsFloat(left, out leftNumber) && Common.IsConstant(right, out rightNumber)) ||
				(Common.IsNumber(left, out leftNumber) && Common.IsNumber(right, out rightNumber)))
			{
				return Expressionizer.ToExpression(new NumericFactor(leftNumber ^ rightNumber));
			}

			ExpressionList leftList;
			ExpressionList rightList;

			if (Common.IsList(left, out leftList))
			{
				return Evaluate(leftList, right);
			}

			if (Common.IsList(right, out rightList))
			{
				return Evaluate(rightList, left);
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

				if (leftNumber is Integer)
				{
					var result = new Float(leftNumber.ValueAsFloat()) ^ rightFraction;
					if (Number.IsWholeNumber(result))
					{
						return Expressionizer.ToExpression(new NumericFactor(new Integer((int)result.ValueAsFloat())));
					}

					return AsExpression(left, right);
				}

				return Expressionizer.ToExpression(leftNumber ^ rightFraction);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsNumber(right, out rightNumber))
			{
				// Special case: negative left ^ fractional right (any number with a decimal portion) is non-real
				if (leftFraction.IsNegative && !Number.IsWholeNumber(rightNumber))
				{
					throw new NonRealResultException();
				}

				return Expressionizer.ToExpression(leftFraction ^ rightNumber);
			}

			if (Common.IsIntegerFraction(left, out leftFraction) && Common.IsIntegerFraction(right, out rightFraction))
			{
				// Special case: negative left ^ fractional right (any number with a decimal portion) is non-real
				if (leftFraction.IsNegative && rightFraction.Denominator > 1)
				{
					throw new NonRealResultException();
				}

				return Expressionizer.ToExpression(leftFraction ^ rightFraction);
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
					var result = leftFraction ^ rightInteger;
					return Expressionizer.ToExpression(result);
				}
				else
				{
					var rightDouble = (Float)rightNumber;
					var result = leftFraction ^ rightDouble;
					return Expressionizer.ToExpression(new NumericFactor(result));
				}
			}

			return AsExpression(left, right);
		}

		internal override string ToString(List<Expression> parameters)
		{
			// Special case for square root
			var powerAsComponent = Componentizer.ToComponent(parameters[1]);
			if (powerAsComponent is IntegerFraction)
			{
				var frac = (IntegerFraction)powerAsComponent;
				if (frac.Numerator == 1 && frac.Denominator == 2)
				{
					return string.Format("√({0})", ToString(parameters[0]));
				}
			}

			return string.Format("{0}^{1}", ToString(parameters[0]), ToString(parameters[1]));
		}

		private static Expression Evaluate(ExpressionList leftNumber, Evaluatable right)
		{
			return Expressionizer.ToExpression(new ExpressionList(leftNumber.Expressions.Select(e => new Exponent().Evaluate(e, Expressionizer.ToExpression(right)).Evaluate()).ToList()));
		}

		private static string ToString(Evaluatable expression)
		{
			if (expression is DualComponentExpression)
			{
				return string.Format("({0})", expression);
			}

			var component = Componentizer.ToComponent(expression);
			if (component is DualFactorComponent || component is ComponentList)
			{
				var output = expression.ToString();
				if (output.Contains("("))
				{
					return string.Format("({0})", expression);
				}
			}

			return expression.ToString();
		}
	}
}
