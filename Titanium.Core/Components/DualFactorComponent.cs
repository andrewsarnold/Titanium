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

				var component = Componentizer.ToComponent(factor);
				if (component is DualFactorComponent || (isDenominator && component is FunctionComponent))
				{
					var output = expression.ToString();
					if (output.Contains("("))
					{
						return string.Format("({0})", expression);
					}
				}
			}

			return factor.ToString();
		}
	}
}
