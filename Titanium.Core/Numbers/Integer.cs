namespace Titanium.Core.Numbers
{
	public class Integer : Number
	{
		public readonly int Value;

		internal Integer(int value)
		{
			Value = value;
		}
		
		public override string ToString()
		{
			return Value.ToString().Replace("-", "⁻");
		}

		protected override double ValueAsFloat()
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

		internal static Integer Zero { get { return new Integer(0); } }

		internal static Integer GreatestCommonDivisor(Integer a, Integer b)
		{
			return b.Value == 0
				? a
				: GreatestCommonDivisor(b, Mod(a, b));
		}

		internal static int GreatestCommonDivisor(int a, int b)
		{
			return b == 0
				? a
				: GreatestCommonDivisor(b, a % b);
		}

		private static Integer Mod(Integer a, Integer b)
		{
			return new Integer(a.Value % b.Value);
		}
	}
}
