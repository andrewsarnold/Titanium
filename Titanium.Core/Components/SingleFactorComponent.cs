using Titanium.Core.Exceptions;
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

		public override int CompareTo(object obj)
		{
			var other = obj as SingleFactorComponent;
			if (other != null)
			{
				return Factor.CompareTo(other.Factor);
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var sfc = other as SingleFactorComponent;
			return sfc != null && Factor.Equals(sfc.Factor);
		}
	}
}
