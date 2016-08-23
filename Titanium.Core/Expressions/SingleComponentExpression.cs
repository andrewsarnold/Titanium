using Titanium.Core.Components;
using Titanium.Core.Exceptions;

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

		internal override Expression Evaluate(bool expand = false)
		{
			return Component.Evaluate(expand);
		}

		public override int CompareTo(object obj)
		{
			var other = obj as SingleComponentExpression;
			if (other != null)
			{
				return Component.CompareTo(other.Component);
			}

			var dce = obj as DualComponentExpression;
			if (dce != null)
			{
				var leftComp = Component.CompareTo(dce.LeftComponent);
				var righttComp = Component.CompareTo(dce.RightComponent);
				return leftComp + righttComp;
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var sce = other as SingleComponentExpression;
			return sce != null && Component.Equals(sce.Component);
		}
	}
}
