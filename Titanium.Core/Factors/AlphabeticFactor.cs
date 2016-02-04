using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Numbers;

namespace Titanium.Core.Factors
{
	internal class AlphabeticFactor : Factor
	{
		internal readonly string Value;

		internal AlphabeticFactor(string value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value;
		}

		public override Expression Evaluate()
		{
			return new SingleComponentExpression(new SingleFactorComponent(Constants.IsNamedConstant(Value)
				? (Factor)new NumericFactor(new Float(Constants.Get(Value)))
				: this));
		}
	}
}
