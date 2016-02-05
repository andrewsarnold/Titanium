using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class AlphabeticFactor : Factor
	{
		internal readonly string Value;

		internal AlphabeticFactor(string value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value;
		}

		public override Expression Evaluate()
		{
			return Expressionizer.ToExpression(this);
		}
	}
}
