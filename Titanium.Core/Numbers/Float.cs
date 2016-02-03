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
			return ValueAsFloat().ToString(CultureInfo.InvariantCulture).Replace("-", "⁻");
		}

		protected override double ValueAsFloat()
		{
			// Coerce to 0
			return Math.Abs(Value) < Constants.Tolerance ? 0 : Value;
		}
	}
}
