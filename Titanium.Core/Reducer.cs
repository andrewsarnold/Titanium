using System;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Core
{
	internal static class Reducer
	{
		internal static Factor GetFactor(object thing)
		{
			if (thing is Factor)
			{
				return ((Factor)thing).Evaluate();
			}

			if (thing is Expression)
			{
				var result = ((Expression)thing).Evaluate();
				if (result is SingleComponentExpression)
				{
					var sce = (SingleComponentExpression)result;
					if (sce.Component is SingleFactorComponent)
					{
						var sfc = (SingleFactorComponent)sce.Component;
						if (sfc.Factor is ExpressionFactor)
						{
							return GetFactor(((ExpressionFactor)sfc.Factor).Expression);
						}
					}
				}

				return new ExpressionFactor(result);
			}

			if (thing is Component)
			{
				return GetFactor(new ExpressionFactor(new SingleComponentExpression((Component)thing).Evaluate()));
			}

			throw new UnexpectedTypeException(thing.GetType());
		}

		internal static Component GetComponent(object thing)
		{
			if (thing is Component)
			{
				return (Component)thing;
			}

			if (thing is Factor)
			{
				return new SingleFactorComponent((Factor)thing);
			}

			if (thing is Expression)
			{
				return new SingleFactorComponent(new ExpressionFactor((Expression)thing));
			}

			throw new UnexpectedTypeException(thing.GetType());
		}
	}
}
