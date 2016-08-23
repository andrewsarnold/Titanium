using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Reducer;

namespace Titanium.Test
{
	[TestClass]
	public class ListExpressionTests
	{
		[TestMethod]
		public void EmptyList()
		{
			var expression = Expression.ParseExpression("{}");
			var list = GetAsExpressionList(expression);

			Assert.AreEqual(0, list.Expressions.Count);
			Assert.AreEqual("{}", list.ToString());
		}

		[TestMethod]
		public void OneElementListTest()
		{
			var expression = Expression.ParseExpression("{1}");
			var list = GetAsExpressionList(expression);

			Assert.AreEqual(1, list.Expressions.Count);
			Assert.AreEqual("{1}", list.ToString());
		}

		[TestMethod]
		public void TwoElementListTest()
		{
			var expression = Expression.ParseExpression("{1,2}");
			var list = GetAsExpressionList(expression);

			Assert.AreEqual(2, list.Expressions.Count);
			Assert.AreEqual("{1,2}", list.ToString());
		}

		[TestMethod]
		public void NestedElementListTest()
		{
			var expression = Expression.ParseExpression("{1,2,{a,b}}");
			var list = GetAsExpressionList(expression);
			Assert.AreEqual(3, list.Expressions.Count);
			var innerList = GetAsExpressionList(list.Expressions[2]);
			Assert.AreEqual(2, innerList.Expressions.Count);
		}

		[TestMethod]
		public void DoubleNestedElementListTest()
		{
			var expression = Expression.ParseExpression("{1,{2,{a,b},4}}");
			var list = GetAsExpressionList(expression);
			Assert.AreEqual(2, list.Expressions.Count);
			var innerList = GetAsExpressionList(list.Expressions[1]);
			Assert.AreEqual(3, innerList.Expressions.Count);
			var innerInnerList = GetAsExpressionList(innerList.Expressions[1]);
			Assert.AreEqual(2, innerInnerList.Expressions.Count);
		}

		[TestMethod]
		public void ListAsFirstFactorTest()
		{
			var expression = Expression.ParseExpression("{1,2}*3");
			var component = (ComponentList)Componentizer.ToComponent(expression);
			Assert.IsTrue(component.Factors[0].Factor is ExpressionList);
			Assert.IsTrue(component.Factors[1].Factor is NumericFactor);
		}

		[TestMethod]
		public void ListAsSecondFactorTest()
		{
			var expression = Expression.ParseExpression("3*{1,2}");
			var component = (ComponentList)Componentizer.ToComponent(expression);
			Assert.IsTrue(component.Factors[0].Factor is NumericFactor);
			Assert.IsTrue(component.Factors[1].Factor is ExpressionList);
		}

		private static ExpressionList GetAsExpressionList(Expression expression)
		{
			Assert.IsTrue(expression is SingleComponentExpression);
			var component = ((SingleComponentExpression)expression).Component;
			Assert.IsTrue(component is SingleFactorComponent);
			var factor = ((SingleFactorComponent)component).Factor;
			Assert.IsTrue(factor is ExpressionList);
			return (ExpressionList)factor;
		}
	}
}
