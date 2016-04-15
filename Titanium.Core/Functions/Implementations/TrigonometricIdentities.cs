using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal static class TrigonometricIdentities
	{
		private static readonly List<TrigonometricIdentity> Identities = new List<TrigonometricIdentity>
		{
			// 0
			new TrigonometricIdentity("sin", new NumericFactor(new Integer(0)), new NumericFactor(new Integer(0))), // sin(0) = 0
			new TrigonometricIdentity("cos", new NumericFactor(new Integer(0)), new NumericFactor(new Integer(1))), // cos(0) = 0
			new TrigonometricIdentity("tan", new NumericFactor(new Integer(0)), new NumericFactor(new Integer(0))), // tan(0) = 0

			// pi / 2
			new TrigonometricIdentity("sin", Factorizer.ToFactor(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(new AlphabeticFactor("π")),
				new ComponentListFactor(new NumericFactor(new Integer(2)), false)
			})), new NumericFactor(new Integer(1))), // sin(pi/2) = 1
			new TrigonometricIdentity("cos", Factorizer.ToFactor(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(new AlphabeticFactor("π")),
				new ComponentListFactor(new NumericFactor(new Integer(2)), false)
			})), new NumericFactor(new Integer(0))), // cos(pi/2) = 0
			new TrigonometricIdentity("tan", Factorizer.ToFactor(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(new AlphabeticFactor("π")),
				new ComponentListFactor(new NumericFactor(new Integer(2)), false)
			})), Factorizer.ToFactor(new UndefinedExpression())), // tan(pi/2) = infinity
		};

		public static Factor Get(string function, Factor input)
		{
			var identity = Identities.FirstOrDefault(i => i.Function == function && i.Input.Equals(input));
			return identity != null ? identity.Output : null;
		}
	}
}
