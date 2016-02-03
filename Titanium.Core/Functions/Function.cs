using System.Collections.Generic;
using Titanium.Core.Components;

namespace Titanium.Core.Functions
{
	internal abstract class Function
	{
		internal readonly string Name;

		public Function(string name)
		{
			Name = name;
		}

		internal abstract Component Evaluate(List<object> parameters);
	}
}