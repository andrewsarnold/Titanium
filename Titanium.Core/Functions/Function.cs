using System.Collections.Generic;
using Titanium.Core.Expressions;

namespace Titanium.Core.Functions
{
	internal abstract class Function
	{
		internal readonly string Name;
		internal readonly int ArgumentCount;

		protected Function(string name, int argumentCount)
		{
			Name = name;
			ArgumentCount = argumentCount;
		}

		public abstract Expression Evaluate(List<Expression> parameters);
		public abstract string ToString(List<Expression> parameters);
	}
}