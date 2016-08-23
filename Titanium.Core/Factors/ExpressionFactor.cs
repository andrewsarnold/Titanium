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

		public override bool Equals(Evaluatable other)
		{
			var ef = other as ExpressionFactor;
			return ef != null && Expression.Equals(ef.Expression);
		}

		internal override int CompareTo(Factor factor, bool isMultiply)
		{
			return 0;
		}
	}
}
