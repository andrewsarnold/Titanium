using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class NumericFactor : Factor
	{
		internal readonly Number Number;

		internal NumericFactor(Number number)
		{
			Number = number;
		}

		public override string ToString()
		{
			return Number.ToString();
		}

		internal override Expression Evaluate(bool expand = false)
		{
			return Expressionizer.ToExpression(new NumericFactor(Number));
		}

		public override int CompareTo(object obj)
		{
			if (obj is Expression || obj is Component || obj is ExpressionFactor || Constants.IsNamedConstant(obj.ToString()))
			{
				return -1;
			}

			if (obj is AlphabeticFactor)
			{
				return 1;
			}

			return 0;
		}

		public override bool Equals(Evaluatable other)
		{
			var nf = other as NumericFactor;
			return nf != null && Number.Equals(nf.Number);
		}

		internal override int CompareTo(Factor factor, bool isMultiply)
		{
			if (factor is AlphabeticFactor)
			{
				return -1 * ((AlphabeticFactor)factor).CompareTo(this, isMultiply);
			}

			if (factor is ExpressionFactor)
			{
				return -1;
			}

			return 0;
		}
	}
}
