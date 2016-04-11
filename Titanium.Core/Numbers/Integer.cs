﻿using System;

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

		internal override bool IsNegative
		{
			get { return Value < 0; }
		}

		internal override bool IsZero
		{
			get { return Value == 0; }
		}

		internal override bool IsOne
		{
			get { return Value == 1; }
		}

		internal static Integer Zero { get { return new Integer(0); } }

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
