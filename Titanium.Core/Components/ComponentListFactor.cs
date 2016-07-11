using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Components
{
	internal class ComponentListFactor : Evaluatable
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

		public override int CompareTo(object obj)
		{
			if (obj is Factor)
			{
				return Factor.CompareTo(obj);
			}

			if (obj is ComponentListFactor)
			{
				return Factor.CompareTo(((ComponentListFactor)obj).Factor);
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var clf = other as ComponentListFactor;
			if (clf != null)
			{
				return IsInNumerator == clf.IsInNumerator && Factor.Equals(clf.Factor);
			}

			return false;
		}
	}
}
