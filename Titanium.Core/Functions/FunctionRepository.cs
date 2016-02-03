using System;
using System.Collections.Generic;
using Titanium.Core.Components;
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

		internal static Component Evaluate(string name, List<object> parameters)
		{
			if (Funcs.ContainsKey(name))
			{
				return Funcs[name].Evaluate(parameters);
			}

			throw new NotImplementedException(name);
		}
	}
}
