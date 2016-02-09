namespace Titanium.Core.Numbers
{
	internal class Infinity : Number
	{
		private readonly bool _isNegative;

		internal Infinity(bool isNegative = false)
		{
			_isNegative = isNegative;
		}

		protected override double ValueAsFloat()
		{
			return IsNegative
				? double.NegativeInfinity
				: double.PositiveInfinity;
		}

		internal override bool IsNegative
		{
			get { return _isNegative; }
		}

		internal override bool IsZero
		{
			get { return false; }
		}

		public override string ToString()
		{
			return _isNegative ? "⁻∞" : "∞";
		}
	}
}
