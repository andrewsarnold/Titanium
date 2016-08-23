using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class SquareRoot : Function
	{
		internal SquareRoot()
			: base("√", 1)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			var parameter = parameters[0].Evaluate();
			return new Exponent().Evaluate(parameter, Expressionizer.ToExpression(new IntegerFraction(1, 2)));
		}

		protected override Expression InnerExpand(params Expression[] parameters)
		{
			throw new System.NotImplementedException();
		}

		internal override string ToString(List<Expression> parameters)
		{
			return string.Format("√({0})", parameters[0]);
		}
	}
}
