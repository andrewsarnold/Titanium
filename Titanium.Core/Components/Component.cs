using Titanium.Core.Expressions;

namespace Titanium.Core.Components
{
	internal abstract class Component : IEvaluatable
	{
		public abstract Expression Evaluate();
	}
}
