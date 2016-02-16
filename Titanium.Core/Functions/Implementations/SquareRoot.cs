using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class SquareRoot : Function
	{
		public SquareRoot()
			: base("√", 1)
		{
		}

		protected override Expression InnerEvaluate(List<Expression> parameters)
		{
			var parameter = parameters[0].Evaluate();
			return new Exponent().Evaluate(new List<Expression> { parameter, Expressionizer.ToExpression(new IntegerFraction(1, 2)) });
		}

		public override string ToString(List<Expression> parameters)
		{
			return string.Format("√({0})", parameters[0]);
		}
	}
}
