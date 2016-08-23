using System;
using System.Collections.Generic;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class AlphabeticFactor : Factor
	{
		internal readonly string Value;

		private readonly Dictionary<string, Expression> _variableMap;

		internal AlphabeticFactor(string value, Dictionary<string, Expression> variableMap = null)
		{
			Value = value;
			_variableMap = variableMap;
		}

		public override string ToString()
		{
			return Value;
		}

		internal override Expression Evaluate(bool expand = false)
		{
			return _variableMap != null && _variableMap.ContainsKey(Value)
				? _variableMap[Value]
				: Expressionizer.ToExpression(this);
		}

		public override int CompareTo(object obj)
		{
			if (obj is Factor)
			{
				return CompareTo((Factor)obj, false);
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var af = other as AlphabeticFactor;
			return af != null && Value == af.Value;
		}

		internal override int CompareTo(Factor factor, bool isMultiply)
		{
			var alphabeticFactor = factor as AlphabeticFactor;
			if (alphabeticFactor != null)
			{
				return string.Compare(Value, alphabeticFactor.Value, StringComparison.Ordinal);
			}

			var numericFactor = factor as NumericFactor;
			if (numericFactor != null)
			{
				return isMultiply ? 1 : -1;
			}

			var expressionFactor = factor as ExpressionFactor;
			if (expressionFactor != null)
			{
				return 0;
			}

			throw new IncomparableTypeException(GetType(), factor.GetType());
		}
	}
}
