using Titanium.Core.Expressions;

namespace Titanium.Core.Components
{
	public abstract class Component : IEvaluatable
	{
		public abstract Expression Evaluate();
	}
}
