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

		internal override Expression Evaluate(bool expand = false)
		{
			return Factor.Evaluate(expand);
		}

		public override int CompareTo(object obj)
		{
			var other = obj as SingleFactorComponent;
			if (other != null)
			{
				return Factor.CompareTo(other.Factor);
			}

			var func = obj as FunctionComponent;
			if (func != null)
			{
				return 1;
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
