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
			{ "sin", new Trigonometric("sin", Math.Sin) },
			{ "cos", new Trigonometric("cos", Math.Cos) },
			{ "tan", new Trigonometric("tan", Math.Tan) }
		};

		internal static Expression Evaluate(string name, List<IEvaluatable> parameters)
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
	}
}
