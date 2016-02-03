using System;
using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Expressions;
using Titanium.Core.Functions.Implementations;

namespace Titanium.Core.Functions
{
	internal static class FunctionRepository
	{
		private static readonly Dictionary<string, Function> Funcs = new Dictionary<string, Function>
		{
			{ "!", new Factorial() },
			{ "sin", new SimpleFloatCalculation("sin", Math.Sin) },
			{ "cos", new SimpleFloatCalculation("cos", Math.Cos) },
			{ "tan", new SimpleFloatCalculation("tan", Math.Tan) },
			{ "√", new SimpleFloatCalculation("√", Math.Sqrt) },
			{ "sqrt", new SimpleFloatCalculation("√", Math.Sqrt) }
		};

		internal static Expression Evaluate(string name, IEnumerable<IEvaluatable> parameters)
		{
			if (Funcs.ContainsKey(name))
			{
				return Funcs[name].Evaluate(parameters.Select(p => p.Evaluate()).ToList());
			}

			throw new NotImplementedException(name);
		}

		internal static bool Contains(string name)
		{
			return Funcs.Any(f => f.Key == name);
		}

		internal static int ArgumentCount(string name)
		{
			if (Contains(name))
			{
				return Funcs[name].ArgumentCount;
			}

			throw new NotImplementedException();
		}
	}
}
