using Titanium.Core.Components;

namespace Titanium.Core.Expressions
{
	internal class SingleComponentExpression : Expression
	{
		internal readonly Component Component;

		public SingleComponentExpression(Component component)
		{
			Component = component;
		}

		public override string ToString()
		{
			return Component.ToString();
		}

		public override Expression Evaluate()
		{
			return new SingleComponentExpression(Component.Evaluate());
		}
	}
}
