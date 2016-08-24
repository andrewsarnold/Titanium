using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;
using Titanium.Core.Functions.Implementations;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

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

		//public static Expression operator *(DualComponentExpression left, DualComponentExpression right)
		//{
		//	// (a+b)*(c+d) == a*c + a*d + b*c + b*d
		//	var first = new ComponentList(new List<ComponentListFactor>
		//	{
		//		new ComponentListFactor(Factorizer.ToFactor(left.LeftComponent)),
		//		new ComponentListFactor(Factorizer.ToFactor(right.LeftComponent))
		//	});
		//	var second = new ComponentList(new List<ComponentListFactor>
		//	{
		//		new ComponentListFactor(Factorizer.ToFactor(left.LeftComponent)),
		//		new ComponentListFactor(Factorizer.ToFactor(right.RightComponent))
		//	});
		//	var third = new ComponentList(new List<ComponentListFactor>
		//	{
		//		new ComponentListFactor(Factorizer.ToFactor(left.RightComponent)),
		//		new ComponentListFactor(Factorizer.ToFactor(right.LeftComponent))
		//	});
		//	var fourth = new ComponentList(new List<ComponentListFactor>
		//	{
		//		new ComponentListFactor(Factorizer.ToFactor(left.RightComponent)),
		//		new ComponentListFactor(Factorizer.ToFactor(right.RightComponent))
		//	});
			
		//	var thirdPlusFourth = new DualComponentExpression(Componentizer.ToComponent(third), Componentizer.ToComponent(fourth), true);
		//	var secondPlusThirdPlusFourth = new DualComponentExpression(Componentizer.ToComponent(second), Componentizer.ToComponent(thirdPlusFourth), true);
		//	var firstPlusSecondPlusThirdPlusFourth = new DualComponentExpression(Componentizer.ToComponent(first), Componentizer.ToComponent(secondPlusThirdPlusFourth), true);
		//	return firstPlusSecondPlusThirdPlusFourth.Evaluate();
		//}

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
