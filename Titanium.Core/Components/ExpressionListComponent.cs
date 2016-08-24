using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Functions.Implementations;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	internal class ExpressionListComponent : Evaluatable
	{
		internal readonly Component Component;
		internal bool IsAdd;

		public ExpressionListComponent(Component component, bool isAdd = true)
		{
			var functionComponent = component as FunctionComponent;
			if (functionComponent?.Function is Negate)
			{
				Component = Componentizer.ToComponent(functionComponent.Operands[0]);
				IsAdd = !isAdd;
			}
			else
			{
				Component = component;
				IsAdd = isAdd;
			}
		}

		internal override Expression Evaluate(bool expand = false)
		{
			return Component.Evaluate(expand);
		}

		public override int CompareTo(object obj)
		{
			if (obj is Component)
			{
				return Component.CompareTo(obj);
			}

			if (obj is ExpressionListComponent)
			{
				return Component.CompareTo(((ExpressionListComponent)obj).Component);
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var elc = other as ExpressionListComponent;
			if (elc != null)
			{
				return IsAdd == elc.IsAdd && Component.Equals(elc.Component);
			}

			return false;
		}
	}
}
