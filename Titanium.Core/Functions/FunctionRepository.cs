using System;
using System.Collections.Generic;
using System.Linq;
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
			{ "sqrt", new SimpleFloatCalculation("√", Math.Sqrt) },
			{ "ceil", new SimpleFloatCalculation("ceil", Math.Ceiling) },
			{ "floor", new SimpleFloatCalculation("floor", Math.Floor) },
			{ "abs", new AbsoluteValue() }
		};

		internal static bool Contains(string name)
		{
			return Funcs.Any(f => f.Key == name);
		}

		internal static Function Get(string name)
		{
			if (Contains(name))
			{
				return Funcs[name];
			}

			throw new NotImplementedException();
		}
	}
}
