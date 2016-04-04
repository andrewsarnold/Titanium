using System;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Components
{
	internal class ComponentListFactor : Evaluatable, IComparable
	{
		internal readonly Factor Factor;
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

		public int CompareTo(object obj)
		{
			if (obj is Factor)
			{
				return Factor.CompareTo(obj);
			}

			if (obj is ComponentListFactor)
			{
				return Factor.CompareTo(((ComponentListFactor)obj).Factor);
			}

			return 1;
		}
	}
}
