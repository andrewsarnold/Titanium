using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	internal class ComponentList : Component
	{
		private readonly List<ComponentListFactor> _factors;

		public ComponentList(SingleFactorComponent component)
		{
			_factors = new List<ComponentListFactor>
			{
				new ComponentListFactor(component.Factor)
			};
		}

		public ComponentList(DualFactorComponent dualFactorComponent)
		{
			_factors = new List<ComponentListFactor>();

			GetComponents(dualFactorComponent.LeftFactor);
			GetComponents(dualFactorComponent.RightFactor);
		}

		private void GetComponents(Factor factor)
		{
			if (!(factor is ExpressionFactor))
			{
				_factors.Add(new ComponentListFactor(factor));
			}
			else
			{
				var leftComp = Componentizer.ToComponent(factor);
				if (leftComp is DualFactorComponent)
				{
					var dfc = (DualFactorComponent)leftComp;
					_factors.Add(new ComponentListFactor(dfc.LeftFactor));
					_factors.Add(new ComponentListFactor(dfc.RightFactor));
				}
			}
		}

		public ComponentList(List<ComponentListFactor> factors)
		{
			_factors = factors;
		}

		internal override Expression Evaluate()
		{
			throw new System.NotImplementedException();
		}
	}

	internal class ComponentListFactor
	{
		internal Factor Factor;
		internal bool IsMultiply;

		public ComponentListFactor(Factor factor, bool isMultiply = true)
		{
			Factor = factor;
			IsMultiply = isMultiply;
		}
	}
}
