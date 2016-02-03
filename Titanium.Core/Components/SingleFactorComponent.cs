using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Core.Components
{
	internal class SingleFactorComponent : Component
	{
		public readonly Factor Factor;

		public SingleFactorComponent(Factor factor)
		{
			Factor = factor;
		}

		public override string ToString()
		{
			return Factor.ToString();
		}

		internal override Component Evaluate()
		{
			var result = Factor.Evaluate();

			if (result is ExpressionFactor)
			{
				var expressionFactor = (ExpressionFactor)result;
				if (expressionFactor.Expression is SingleComponentExpression)
				{
					var singleComponentExpression = (SingleComponentExpression)expressionFactor.Expression;
					if (singleComponentExpression.Component is IntegerFraction)
					{
						return (IntegerFraction)singleComponentExpression.Component;
					}
				}
			}

			if (result is NumericFactor)
			{
				var nf = (NumericFactor)result;
				if (nf.Number is Integer)
				{
					return new IntegerFraction((Integer)nf.Number);
				}
			}

			return new SingleFactorComponent(result);
		}
	}
}
