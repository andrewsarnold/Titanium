using Titanium.Core.Components;
using Titanium.Core.Exceptions;

namespace Titanium.Core.Expressions
{
	internal class DualComponentExpression : Expression
	{
		internal readonly Component LeftComponent;
		internal readonly Component RightComponent;
		internal readonly bool IsAdd;

		internal DualComponentExpression(Component leftComponent, Component rightComponent, bool isAdd)
		{
			IsAdd = isAdd;
			LeftComponent = leftComponent;
			RightComponent = rightComponent;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", LeftComponent, IsAdd ? "+" : "-", RightComponent);
		}

		internal override Expression Evaluate(bool expand = false)
		{
			return new ExpressionList(this).Evaluate(expand);
		}

		public override int CompareTo(object obj)
		{
			var other = obj as DualFactorComponent;
			if (other != null)
			{

			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var dce = other as DualComponentExpression;
			if (dce != null)
			{
				return LeftComponent.Equals(dce.LeftComponent) &&
				       RightComponent.Equals(dce.RightComponent) &&
				       IsAdd == dce.IsAdd;
			}

			return false;
		}
	}
}
