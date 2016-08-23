using System;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class StringFactor : Factor
	{
		private readonly string _value;

		internal StringFactor(string value)
		{
			_value = value.Substring(1, value.Length - 2); // Strip outer quote marks
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{0}", "\"", _value);
		}

		internal override Expression Evaluate(bool expand = false)
		{
			return Expressionizer.ToExpression(this);
		}

		public override int CompareTo(object obj)
		{
			var stringFactor = obj as StringFactor;
			if (stringFactor != null)
			{
				return string.Compare(_value, stringFactor._value, StringComparison.Ordinal);
			}

			var alphabeticFactor = obj as AlphabeticFactor;
			if (alphabeticFactor != null)
			{
				return 1;
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
			var sf = other as StringFactor;
			return sf != null && _value == sf._value;
		}

		internal override int CompareTo(Factor factor, bool isMultiply)
		{
			if (factor is NumericFactor)
			{
				return isMultiply ? 1 : -1;
			}

			if (factor is AlphabeticFactor)
			{
				return 1;
			}

			return 0;
		}
	}
}
