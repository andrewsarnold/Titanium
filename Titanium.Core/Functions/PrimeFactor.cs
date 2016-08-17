using System;
using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions
{
	internal class PrimeFactor : Function
	{
		public PrimeFactor()
			: base("factor", 1)
		{
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			var parameter = parameters[0].Evaluate();
			var asFactor = Factorizer.ToFactor(parameter);
			if (asFactor is NumericFactor)
			{
				return Expressionizer.ToExpression(asFactor);
			}

			return parameter;
		}

		internal override string ToString(List<Expression> parameters)
		{
			throw new NotImplementedException();
		}
	}
}
