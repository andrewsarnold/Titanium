using System.Collections.Generic;
using Titanium.Core.Expressions;

namespace Titanium.Core.Functions
{
	internal abstract class Function
	{
		internal readonly string Name;

		protected Function(string name)
		{
			Name = name;
		}

		public abstract Expression Evaluate(List<Expression> parameters);
	}
}