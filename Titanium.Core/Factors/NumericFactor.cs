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

		internal override Expression Evaluate()
		{
			return Expressionizer.ToExpression(new NumericFactor(Number));
		}
	}
}
