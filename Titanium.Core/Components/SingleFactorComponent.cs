using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Components
{
	internal class SingleFactorComponent : Component
	{
		public readonly Factor Factor;

		public SingleFactorComponent(Factor factor)
		{
			Factor = factor;
		}

		public override string ToString()
		{
			return Factor.ToString();
		}

		public override Expression Evaluate()
		{
			return Factor.Evaluate();
		}
	}
}
