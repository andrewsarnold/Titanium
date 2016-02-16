using System;
using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Functions.Implementations;

namespace Titanium.Core.Functions
{
	public static class FunctionRepository
	{
		private static readonly Dictionary<string, Function> Funcs = new Dictionary<string, Function>
		{
			{ "⁻", new Negate() },
			{ "^", new Exponent() },
			{ "!", new Factorial() },
			{ "sin", new SimpleFloatCalculation("sin", Math.Sin) },
			{ "cos", new SimpleFloatCalculation("cos", Math.Cos) },
			{ "tan", new SimpleFloatCalculation("tan", Math.Tan) },
			{ "√", new SquareRoot() },
			{ "sqrt", new SimpleFloatCalculation("√", Math.Sqrt) },
			{ "ceiling", new SimpleFloatCalculation("ceiling", Math.Ceiling) },
			{ "floor", new SimpleFloatCalculation("floor", Math.Floor) },
			{ "abs", new AbsoluteValue() },
			{ "ln", new NaturalLog() },
			{ "log", new BaseTenLogarithm() }
		};

		public static IEnumerable<string> AllNames
		{
			get { return Funcs.Select(f => f.Key); }
		}

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
