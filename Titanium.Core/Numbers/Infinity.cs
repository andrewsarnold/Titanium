namespace Titanium.Core.Numbers
{
	internal class Infinity : Number
	{
		private readonly bool _isNegative;

		internal Infinity(bool isNegative = false)
		{
			_isNegative = isNegative;
		}

		internal override double ValueAsFloat()
		{
			return IsNegative
				? double.NegativeInfinity
				: double.PositiveInfinity;
		}

		internal override Number AbsoluteValue()
		{
			return new Infinity();
		}

		internal override bool IsNegative => _isNegative;
		internal override bool IsZero => false;
		internal override bool IsOne => false;

		public override bool Equals(Number other)
		{
			var inf = other as Infinity;
			return inf != null && _isNegative == inf.IsNegative;
		}

		public override string ToString()
		{
			return _isNegative ? "⁻∞" : "∞";
		}
	}
}
