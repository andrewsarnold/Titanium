using System;
using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Functions.Implementations;
using Titanium.Core.Numbers;
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
			var numericFactor = Factorizer.ToFactor(parameter) as NumericFactor;
			if (numericFactor == null)
			{
				return parameter;
			}
			if (numericFactor.Number.IsZero)
			{
				return Expressionizer.ToExpression(numericFactor);
			}
			if (numericFactor.Number.IsOne)
			{
				return Expressionizer.ToExpression(new NumericFactor(new Integer(1)));
			}

			var number = Math.Abs(numericFactor.Number.ValueAsFloat());

			// TODO: If number is fractional and greater than 1,
			// convert to an integer fraction and go from there.
			if (number > 1 && number % 1 > 0)
			{
				return Expressionizer.ToExpression(numericFactor);
			}

			var isFractional = false;
			if (number > 0 && number < 1)
			{
				number = 1 / number;
				isFractional = true;
			}

			var plt = PrimesLessThan(number);
			var primeCounts = new Dictionary<int, int>();
			foreach (var prime in plt)
			{
				while (number % prime < Constants.Tolerance)
				{
					if (!primeCounts.ContainsKey(prime))
					{
						primeCounts.Add(prime, 1);
					}
					else
					{
						primeCounts[prime]++;
					}

					number /= prime;
				}
			}

			var useFloats = numericFactor.Number is Float;
			var unevaluatedExponentFunctions = primeCounts.Select(pc => pc.Value > 1
				? new FunctionComponent(new Exponent(), new List<Expression>
				{
					Expressionizer.ToExpression(new NumericFactor(useFloats ? (Number)new Float(pc.Key) : new Integer(pc.Key))),
					Expressionizer.ToExpression(new NumericFactor(new Integer(pc.Value)))
				})
				: Componentizer.ToComponent(new NumericFactor(useFloats ? (Number)new Float(pc.Key) : new Integer(pc.Key))));
			var componentList = new ComponentList(unevaluatedExponentFunctions.Select(uef => new ComponentListFactor(Factorizer.ToFactor(uef))).ToList());

			if (isFractional)
			{
				foreach (var componentListFactor in componentList.Factors)
				{
					componentListFactor.IsInNumerator = false;
				}

				if (!numericFactor.Number.IsNegative)
				{
					componentList.Factors.Insert(0, new ComponentListFactor(new NumericFactor(new Integer(1))));
				}
			}
			
			if (numericFactor.Number.IsNegative)
			{
				componentList.Factors.Insert(0, new ComponentListFactor(new NumericFactor(useFloats
					? (Number)new Float(-1)
					: new Integer(-1))));
			}

			return Expressionizer.ToExpression(componentList);
		}

		internal override string ToString(List<Expression> parameters)
		{
			throw new NotImplementedException();
		}

		private static IEnumerable<int> PrimesLessThan(double value)
		{
			var sieve = Enumerable.Range(2, (int)value + 1).ToList();

			for (var i = 0; i < sieve.Count; i++)
			{
				var thisPrime = sieve[i];
				sieve.RemoveAll(s => s != thisPrime && s % thisPrime == 0);
			}

			return sieve;
		}
	}
}
