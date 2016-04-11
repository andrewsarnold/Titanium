using System;
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

		internal override Expression Evaluate()
		{
			return Expressionizer.ToExpression(this);
		}

		public override int CompareTo(object obj)
		{
			if (obj is AlphabeticFactor)
			{
				return String.Compare(Value, ((AlphabeticFactor)obj).Value, StringComparison.Ordinal);
			}

			return 0;
		}
	}
}
