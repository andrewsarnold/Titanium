using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Components
{
	internal class ComponentListFactor : Evaluatable
	{
		internal Factor Factor;
		internal bool IsMultiply;

		public ComponentListFactor(Factor factor, bool isMultiply = true)
		{
			Factor = factor;
			IsMultiply = isMultiply;
		}

		internal override Expression Evaluate()
		{
			return Factor.Evaluate();
		}
	}
}
