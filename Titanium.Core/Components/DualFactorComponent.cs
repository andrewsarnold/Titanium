using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	internal class DualFactorComponent : Component
	{
		internal readonly Factor LeftFactor;
		internal readonly Factor RightFactor;
		internal readonly bool IsMultiply;

		internal DualFactorComponent(Factor leftFactor, Factor rightFactor, bool isMultiply)
		{
			IsMultiply = isMultiply;
			LeftFactor = leftFactor;
			RightFactor = rightFactor;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", ToString(LeftFactor),
				IsMultiply ? "*" : "/",
				ToString(RightFactor, !IsMultiply));
		}

		internal override Expression Evaluate()
		{
			return new ComponentList(this).Evaluate();
		}

		private static string ToString(Evaluatable factor, bool isDenominator = false)
		{
			if (factor is ExpressionFactor)
			{
				var expression = Expressionizer.ToExpression(factor);
				if (expression is DualComponentExpression)
				{
					return string.Format("({0})", expression);
				}

				var component = Componentizer.ToComponent(expression);
				if (component is DualFactorComponent || component is ComponentList || component is IntegerFraction || (isDenominator && component is FunctionComponent))
				{
					return string.Format("({0})", component);
				}
			}

			return factor.ToString();
		}
	}
}
