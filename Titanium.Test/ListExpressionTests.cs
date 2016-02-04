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
			Assert.IsTrue(expression is SingleComponentExpression);
			var component = ((SingleComponentExpression)expression).Component;
			Assert.IsTrue(component is SingleFactorComponent);
			var factor = ((SingleFactorComponent)component).Factor;
			Assert.IsTrue(factor is ExpressionList);
			var list = (ExpressionList)factor;

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("{1}", list.ToString());
		}

		[TestMethod]
		public void TwoElementListTest()
		{
			var expression = Expression.ParseExpression("{1,2}");
			Assert.IsTrue(expression is SingleComponentExpression);
			var component = ((SingleComponentExpression)expression).Component;
			Assert.IsTrue(component is SingleFactorComponent);
			var factor = ((SingleFactorComponent)component).Factor;
			Assert.IsTrue(factor is ExpressionList);
			var list = (ExpressionList)factor;

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("{1,2}", list.ToString());
		}
	}
}
