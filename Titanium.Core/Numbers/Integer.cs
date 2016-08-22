using System;

namespace Titanium.Core.Numbers
{
	internal class Integer : Number
	{
		internal readonly int Value;

		internal Integer(int value)
		{
			Value = value;
		}
		
		public override string ToString()
		{
			return Value.ToString().Replace("-", "⁻");
		}

		internal override double ValueAsFloat()
		{
			return Value;
		}

		internal override Number AbsoluteValue()
		{
			return new Integer(Math.Abs(Value));
		}

		internal override bool IsNegative => Value < 0;
		internal override bool IsZero => Value == 0;
		internal override bool IsOne => Value == 1;

		public override bool Equals(Number other)
		{
			var integer = other as Integer;
			return integer != null && Value == integer.Value;
		}

		internal static Integer Zero => new Integer(0);

		internal static int LeastCommonMultiple(int a, int b)
		{
			return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
		}

		internal static int GreatestCommonDivisor(int a, int b)
		{
			return b == 0
				? a
				: GreatestCommonDivisor(b, a % b);
		}
	}
}
