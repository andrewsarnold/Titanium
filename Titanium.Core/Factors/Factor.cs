using System;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Numbers;
using Titanium.Core.Tokens;

namespace Titanium.Core.Factors
{
	internal abstract class Factor : Evaluatable, IComparable
	{
		internal static NumericFactor GetIntegerFactor(Token token)
		{
			var stringVal = token.Value.Replace("⁻", "-");
			return new NumericFactor(new Integer(int.Parse(stringVal)));
		}

		internal static NumericFactor GetFloatFactor(Token token)
		{
			var stringVal = token.Value.Replace("⁻", "-");
			return new NumericFactor(new Float(double.Parse(stringVal)));
		}

		public abstract int CompareTo(object obj);
	}
}
