using Titanium.Core.Components;

namespace Titanium.Core.Expressions
{
	internal class SingleComponentExpression : Expression
	{
		internal readonly Component Component;

		internal SingleComponentExpression(Component component)
		{
			Component = component;
		}

		public override string ToString()
		{
			return Component.ToString();
		}

		internal override Expression Evaluate()
		{
			return Component.Evaluate();
		}
	}
}
