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

		public override Expression Evaluate()
		{
			var result = Factor.Evaluate();

			if (result is SingleComponentExpression)
			{
				var component = ((SingleComponentExpression) result).Component;
				if (component is SingleFactorComponent)
				{
					var factor = ((SingleFactorComponent) component).Factor;

					if (factor is ExpressionFactor)
					{
						var expressionFactor = (ExpressionFactor)factor;
						if (expressionFactor.Expression is SingleComponentExpression)
						{
							var singleComponentExpression = (SingleComponentExpression)expressionFactor.Expression;
							if (singleComponentExpression.Component is IntegerFraction)
							{
								return Common.ToExpression((IntegerFraction)singleComponentExpression.Component);
							}
						}
					}

					if (factor is NumericFactor)
					{
						var nf = (NumericFactor)factor;
						if (nf.Number is Integer)
						{
							return Common.ToExpression(new IntegerFraction((Integer)nf.Number));
						}
					}
				}
			}

			return result;
		}
	}
}
