using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Tokens;

namespace Titanium.Test
{
	[TestClass]
	public class ExpressionTests
	{
		[TestMethod]
		public void ZeroExpressionTest()
		{
			var exp = new SingleComponentExpression(new SingleFactorComponent(new NumericFactor(new Integer(0))));
			Assert.AreEqual("0", exp.ToString());
		}

		[TestMethod]
		public void NumberTest()
		{
			var two = new Integer(2);
			Assert.AreEqual(2, two.Value);
			Assert.AreEqual("2", two.ToString());

			var twopointfour = new Float(2.4);
			Assert.AreEqual(2.4, twopointfour.Value);
			Assert.AreEqual("2.4", twopointfour.ToString());
		}

		[TestMethod]
		public void FactorDoubleTest()
		{
			var factor = Factor.GetFloatFactor(new Token(TokenType.Float, "2.03"));
			Assert.IsTrue(factor.Number is Float);
			Assert.AreEqual(2.03, ((Float)factor.Number).Value);
		}

		[TestMethod]
		public void FactorIntegerTest()
		{
			var factor = Factor.GetIntegerFactor(new Token(TokenType.Integer, "⁻44"));
			Assert.IsTrue(factor.Number is Integer);
			Assert.AreEqual(-44, ((Integer)factor.Number).Value);
		}

		[TestMethod]
		public void IntegerFractionTest()
		{
			var f1 = new IntegerFraction(1, 2);
			var f2 = new IntegerFraction(3, 5);
			var f3 = new IntegerFraction(4, 8);
			var f4 = new IntegerFraction(4, 2);
			var f5 = new IntegerFraction(-2, 3);
			var f6 = new IntegerFraction(-1, 6);

			Assert.AreEqual("1/2", f1.ToString());
			Assert.AreEqual("3/5", f2.ToString());
			Assert.AreEqual("1/2", f3.ToString());
			Assert.AreEqual("2", f4.ToString());
			Assert.AreEqual("⁻2/3", f5.ToString());

			Assert.AreEqual("11/10", (f1 + f2).ToString());
			Assert.AreEqual("1", (f1 + f3).ToString());
			Assert.AreEqual("5/2", (f1 + f4).ToString());
			Assert.AreEqual("⁻1/6", (f1 + f5).ToString());

			Assert.AreEqual("11/10", (f2 + f3).ToString());
			Assert.AreEqual("13/5", (f2 + f4).ToString());
			Assert.AreEqual("⁻1/15", (f2 + f5).ToString());

			Assert.AreEqual("⁻1/10", (f1 - f2).ToString());
			Assert.AreEqual("0", (f1 - f3).ToString());
			Assert.AreEqual("⁻3/2", (f1 - f4).ToString());
			Assert.AreEqual("7/6", (f1 - f5).ToString());

			Assert.AreEqual("3/10", (f1 * f2).ToString());
			Assert.AreEqual("1/4", (f1 * f3).ToString());
			Assert.AreEqual("1", (f1 * f4).ToString());
			Assert.AreEqual("⁻1/3", (f1 * f5).ToString());
			Assert.AreEqual("1/9", (f5 * f6).ToString());

			Assert.AreEqual("5/6", (f1 / f2).ToString());
			Assert.AreEqual("1", (f1 / f1).ToString());
			Assert.AreEqual("1", (f1 / f3).ToString());
			Assert.AreEqual("1/4", (f1 / f4).ToString());
			Assert.AreEqual("⁻3/4", (f1 / f5).ToString());
		}
	}
}
