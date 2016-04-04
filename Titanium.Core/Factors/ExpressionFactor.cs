using Titanium.Core.Expressions;

namespace Titanium.Core.Factors
{
	internal class ExpressionFactor : Factor
	{
		internal readonly Expression Expression;

		internal ExpressionFactor(Expression expression)
		{
			Expression = expression;
		}

		public override string ToString()
		{
			return Expression is DualComponentExpression
				? string.Format("({0})", Expression)
				: Expression.ToString();
		}

		internal override Expression Evaluate()
		{
			return Expression.Evaluate();
		}

		public override int CompareTo(object obj)
		{
			if (obj is Expression)
			{
				return -1;
			}

			return 0;
		}
	}
}
