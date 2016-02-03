using Titanium.Core.Numbers;

namespace Titanium.Core.Factors
{
	internal class NumericFactor : Factor
	{
		internal readonly Number Number;

		internal NumericFactor(Number number)
		{
			Number = number;
		}

		public override string ToString()
		{
			return Number.ToString();
		}

		internal override Factor Evaluate()
		{
			return new NumericFactor(Number);
		}
	}
}
