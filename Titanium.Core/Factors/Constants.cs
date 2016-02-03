using System;
using System.Collections.Generic;
using System.Linq;

namespace Titanium.Core.Factors
{
	internal static class Constants
	{
		internal const double Tolerance = 1E-15;

		private static readonly Dictionary<string, double> Dictionary = new Dictionary<string, double>
		{
			{ "π", Math.PI },
			{ "e", Math.E }
		};

		internal static bool IsNamedConstant(string input)
		{
			return Dictionary.Any(c => c.Key == input);
		}

		internal static double Get(string input)
		{
			return Dictionary.First(c => c.Key == input).Value;
		}
	}
}
