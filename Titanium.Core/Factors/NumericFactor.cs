using Titanium.Core.Components;
using Titanium.Core.Expressions;
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

		public override Expression Evaluate()
		{
			return new SingleComponentExpression(new SingleFactorComponent(new NumericFactor(Number)));
		}
	}
}
