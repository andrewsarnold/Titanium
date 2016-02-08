using System;
using System.Globalization;
using Titanium.Core.Factors;

namespace Titanium.Core.Numbers
{
	internal class Float : Number
	{
		public readonly double Value;

		public Float(double value)
		{
			Value = value;
		}

		public override string ToString()
		{
			var value = ValueAsFloat().ToString(CultureInfo.InvariantCulture).Replace("-", "⁻");
			return value.Contains(".")
				? value
				: string.Format("{0}.",value);
		}

		protected override double ValueAsFloat()
		{
			// Coerce to 0
			return Math.Abs(Value) < Constants.Tolerance ? 0 : Value;
		}

		internal override bool IsNegative
		{
			get { return Value < 0; }
		}

		internal static bool IsWholeNumber(double f)
		{
			return Math.Abs(f % 1) < Constants.Tolerance;
		}

		internal static bool IsWholeNumber(Float f)
		{
			return IsWholeNumber(f.Value);
		}
	}
}
