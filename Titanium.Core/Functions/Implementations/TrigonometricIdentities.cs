using System;
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
			new TrigonometricIdentity("cos", new NumericFactor(new Integer(0)), new NumericFactor(new Integer(1))), // cos(0) = 1

			// pi / 8
			new TrigonometricIdentity("sin", Factorizer.ToFactor(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(new AlphabeticFactor("π")),
				new ComponentListFactor(new NumericFactor(new Integer(8)), false)
			})), Factorizer.ToFactor(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(Factorizer.ToFactor(new FunctionComponent("√", new List<Expression>
				{
					new DualComponentExpression(Componentizer.ToComponent(new NumericFactor(new Integer(2))),
						new FunctionComponent("√", new List<Expression> { Expressionizer.ToExpression(new NumericFactor(new Integer(2))) }), false)
				}))),
				new ComponentListFactor(new NumericFactor(new Integer(2)), false)
			}))), // sin(pi/8) = √(2-√(2))/2
			new TrigonometricIdentity("cos", Factorizer.ToFactor(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(new AlphabeticFactor("π")),
				new ComponentListFactor(new NumericFactor(new Integer(8)), false)
			})), Factorizer.ToFactor(new ComponentList(new List<ComponentListFactor>
			{
				new ComponentListFactor(Factorizer.ToFactor(new FunctionComponent("√", new List<Expression>
				{
					new DualComponentExpression(new FunctionComponent("√", new List<Expression> { Expressionizer.ToExpression(new NumericFactor(new Integer(2))) }),
						Componentizer.ToComponent(new NumericFactor(new Integer(2))), true)
				}))),
				new ComponentListFactor(new NumericFactor(new Integer(2)), false)
			}))), // cos(pi/8) = √(√(2)+2)/2

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
		};

		public static Factor Get(string function, Factor input)
		{
			if (function == "tan")
			{
				var sin = Get("sin", input);
				var cos = Get("cos", input);
				if (sin != null && cos != null)
				{
					try
					{
						return new ExpressionFactor(new SingleComponentExpression(new ComponentList(new List<ComponentListFactor>
						{
							new ComponentListFactor(sin),
							new ComponentListFactor(cos, false)
						})).Evaluate());
					}
					catch (DivideByZeroException)
					{
						return Factorizer.ToFactor(new UndefinedExpression());
					}
				}
			}

			var identity = Identities.FirstOrDefault(i => i.Function == function && i.Input.Equals(input));
			return identity != null ? identity.Output : null;
		}
	}
}
