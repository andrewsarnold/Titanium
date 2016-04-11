using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Components
{
	internal class SingleFactorComponent : Component
	{
		internal readonly Factor Factor;

		internal SingleFactorComponent(Factor factor)
		{
			Factor = factor;
		}

		public override string ToString()
		{
			return Factor.ToString();
		}

		internal override Expression Evaluate()
		{
			return Factor.Evaluate();
		}
	}
}
