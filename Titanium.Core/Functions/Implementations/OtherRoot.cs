using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class OtherRoot : Function
	{
		internal OtherRoot()
			: base("root", 2)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			return new Exponent().Evaluate(parameters[1], Expressionizer.ToExpression(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(Factorizer.ToFactor(parameters[0]), false)
			})));
		}

		protected override Expression InnerExpand(params Expression[] parameters)
		{
			throw new System.NotImplementedException();
		}

		internal override string ToString(List<Expression> parameters)
		{
			return string.Format("root({0},{1})", parameters[0], parameters[1]);
		}
	}
}
