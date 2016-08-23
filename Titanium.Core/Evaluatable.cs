using System;
using Titanium.Core.Expressions;

namespace Titanium.Core
{
	internal abstract class Evaluatable : IComparable, IEquatable<Evaluatable>
	{
		internal abstract Expression Evaluate(bool expand = false);
		public abstract int CompareTo(object obj);
		public abstract bool Equals(Evaluatable other);
	}
}
