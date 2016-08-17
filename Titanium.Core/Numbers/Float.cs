using System;
using System.Globalization;
using System.Text.RegularExpressions;
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
			// Should be a setting
			const int showDigits = 6;
			var showInScientificNotation = ValueAsFloat() >= (10 ^ showDigits);

			string value;
			if (showInScientificNotation)
			{
				value = ValueAsFloat().ToString(string.Format("G{0}", showDigits), CultureInfo.InvariantCulture).Replace("-", "⁻");
				value = Regex.Replace(value, "(E\\+0*)", ".E");
				value = Regex.Replace(value, "\\.$", "");
			}
			else
			{
				value = ValueAsFloat().ToString(CultureInfo.InvariantCulture);
			}

			value = value.Replace("-", "⁻");

			return value.Contains(".")
				? StripTrailingZeros(StripLeadingZeros(value))
				: string.Format("{0}.", value);
		}

		internal override double ValueAsFloat()
		{
			// Coerce to 0
			return Math.Abs(Value) < Constants.Tolerance ? 0 : Value;
		}

		internal override Number AbsoluteValue()
		{
			return new Float(Math.Abs(Value));
		}

		internal override bool IsNegative
		{
			get { return Value < 0; }
		}

		internal override bool IsZero
		{
			get { return Math.Abs(Value) < Constants.Tolerance; }
		}

		internal override bool IsOne
		{
			get { return Math.Abs(Value - 1) < Constants.Tolerance; }
		}

		public override bool Equals(Number other)
		{
			var f = other as Float;
			return f != null && Math.Abs(Value - f.Value) < Constants.Tolerance;
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

		private static string StripLeadingZeros(string value)
		{
			return value.TrimStart('0');
		}

		private static string StripTrailingZeros(string value)
		{
			return value.TrimEnd('0');
		}
	}
}
