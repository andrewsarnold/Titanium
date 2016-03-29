using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Components
{
	internal class ComponentListFactor : Evaluatable
	{
		internal Factor Factor;
		internal bool IsInNumerator;

		public ComponentListFactor(Factor factor, bool isInNumerator = true)
		{
			Factor = factor;
			IsInNumerator = isInNumerator;
		}

		internal override Expression Evaluate()
		{
			return Factor.Evaluate();
		}
	}
}
