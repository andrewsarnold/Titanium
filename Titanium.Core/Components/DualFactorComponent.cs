using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core.Components
{
	internal class DualFactorComponent : Component
	{
		internal readonly Factor LeftFactor;
		internal readonly Factor RightFactor;
		internal readonly bool IsMultiply;

		internal DualFactorComponent(Factor leftFactor, Factor rightFactor, bool isMultiply)
		{
			IsMultiply = isMultiply;
			LeftFactor = leftFactor;
			RightFactor = rightFactor;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", LeftFactor,
				IsMultiply ? "*" : "/",
				RightFactor);
		}

		internal override Expression Evaluate()
		{
			return new ComponentList(this).Evaluate();
		}

		public override int CompareTo(object obj)
		{
			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var dfc = other as DualFactorComponent;
			if (dfc != null)
			{
				return LeftFactor.Equals(dfc.LeftFactor) &&
				       RightFactor.Equals(dfc.RightFactor) &&
				       IsMultiply == dfc.IsMultiply;
			}

			return false;
		}
	}
}
