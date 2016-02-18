using System;
using System.Globalization;
using Titanium.Core.Factors;

namespace Titanium.Core.Numbers
{
	internal class Float : Number
	{
		internal readonly double Value;

		internal Float(double value)
		{
			Value = value;
		}

		public override string ToString()
		{
			var value = ValueAsFloat().ToString(CultureInfo.InvariantCulture).Replace("-", "⁻");
			return value.Contains(".")
				? StripTrailingZeros(StripLeadingZeros(value))
				: string.Format("{0}.",value);
		}

		internal override double ValueAsFloat()
		{
			// Coerce to 0
			return Math.Abs(Value) < Constants.Tolerance ? 0 : Value;
		}

		internal override bool IsNegative
		{
			get { return Value < 0; }
		}

		internal override bool IsZero
		{
			get { return Math.Abs(Value) < Constants.Tolerance; }
		}

		internal static bool IsWholeNumber(double f)
		{
			return Math.Abs(f % 1) < Constants.Tolerance ||
				double.IsInfinity(f) ||
				double.IsNegativeInfinity(f);
		}

		internal static bool IsWholeNumber(Float f)
		{
			return IsWholeNumber(f.Value);
		}

		private string StripLeadingZeros(string value)
		{
			return value.TrimStart('0');
		}

		private string StripTrailingZeros(string value)
		{
			return value.TrimEnd('0');
		}
	}
}
