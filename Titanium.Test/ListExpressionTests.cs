using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Components;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;

namespace Titanium.Test
{
	[TestClass]
	public class ListExpressionTests
	{
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
