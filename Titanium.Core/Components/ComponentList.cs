using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	internal class ComponentList : Component
	{
		internal readonly List<ComponentListFactor> Factors;

		public ComponentList(Component component)
		{
			Factors = GetComponents(component);
		}

		private List<ComponentListFactor> GetComponents(Component component)
		{
			if (component is SingleFactorComponent)
			{
				return new List<ComponentListFactor> { new ComponentListFactor(((SingleFactorComponent)component).Factor) };
			}

			var dfc = (DualFactorComponent)component;
			var leftComponent = Componentizer.ToComponent(dfc.LeftFactor);
			var rightComponent = Componentizer.ToComponent(dfc.RightFactor);

			var leftList = GetComponents(leftComponent);
			var rightList = GetComponents(rightComponent);

			return leftList.Union(rightList).ToList();
		}

		internal override Expression Evaluate()
		{
			throw new System.NotImplementedException();
		}

		public override string ToString()
		{
			return string.Join("*", Factors.Select(f => f.Factor));
		}
	}

	internal class ComponentListFactor
	{
		internal readonly Factor Factor;
		internal readonly bool IsMultiply;

		public ComponentListFactor(Factor factor, bool isMultiply = true)
		{
			Factor = factor;
			IsMultiply = isMultiply;
		}
	}
}
