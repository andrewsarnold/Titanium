using Titanium.Core.Numbers;
using Titanium.Core.Tokens;

namespace Titanium.Core.Factors
{
	internal abstract class Factor
	{
		internal static NumericFactor GetNumericFactor(Token token)
		{
			var stringVal = token.Value.Replace("⁻", "-");

			return stringVal.Contains(".")
				? new NumericFactor(new Float(double.Parse(stringVal)))
				: new NumericFactor(new Integer(int.Parse(stringVal)));
		}

		internal abstract Factor Evaluate();
	}
}
