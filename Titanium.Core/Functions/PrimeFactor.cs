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
			var asFactor = Factorizer.ToFactor(parameter);
			var numericFactor = asFactor as NumericFactor;
			if (numericFactor != null)
			{
				var number = Math.Abs(numericFactor.Number.ValueAsFloat());
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

				var exponents = primeCounts.Select(pc => pc.Value > 1
					? string.Format("{0}^{1}", pc.Key, pc.Value)
					: pc.Key.ToString());
				var expression = string.Join("*", exponents);
				
				return Expressionizer.ToExpression(numericFactor);
			}

			return parameter;
		}

		internal override string ToString(List<Expression> parameters)
		{
			throw new NotImplementedException();
		}

		private static IEnumerable<int> PrimesLessThan(double value)
		{
			var max = (int)Math.Sqrt(value) + 1;
			var sieve = Enumerable.Range(2, max).ToList();

			for (var i = 0; i < sieve.Count; i++)
			{
				var thisPrime = sieve[i];
				sieve.RemoveAll(s => s != thisPrime && s % thisPrime == 0);
			}

			return sieve;
		}
	}
}
