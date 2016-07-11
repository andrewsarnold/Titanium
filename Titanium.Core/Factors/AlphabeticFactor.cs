using System;
using Titanium.Core.Exceptions;
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
			var alphabeticFactor = obj as AlphabeticFactor;
			if (alphabeticFactor != null)
			{
				return String.Compare(Value, alphabeticFactor.Value, StringComparison.Ordinal);
			}

			var numericFactor = obj as NumericFactor;
			if (numericFactor != null)
			{
				return 1;
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var af = other as AlphabeticFactor;
			return af != null && Value == af.Value;
		}
	}
}
