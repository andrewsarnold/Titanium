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

		internal override Expression Evaluate(bool expand = false)
		{
			return Expression.Evaluate(expand);
		}

		public override int CompareTo(object obj)
		{
			if (obj is Expression || obj is NumericFactor || obj is AlphabeticFactor)
			{
				return -1;
			}

			if (obj is ExpressionFactor)
			{
				return Expression.CompareTo(((ExpressionFactor)obj).Expression);
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
