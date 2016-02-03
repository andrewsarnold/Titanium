using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Expressions;

namespace Titanium.Test
{
	[TestClass]
	public class ExpressionBuilderTests
	{
		[TestMethod]
		public void NumericExpression()
		{
			var expression = Expression.ParseExpression("12");
			Assert.AreEqual("12", expression.ToString());
		}

		[TestMethod]
		public void DualComponentExpression()
		{
			var expression = Expression.ParseExpression("12 + 4");
			Assert.AreEqual("12+4", expression.ToString());
		}

		[TestMethod]
		public void ParentheticalExpression()
		{
			var expression = Expression.ParseExpression("(12 + 4)");
			Assert.AreEqual("12+4", expression.ToString());
		}

		[TestMethod]
		public void NestedParentheticalExpression()
		{
			var expression = Expression.ParseExpression("(12 + (3 * 4))");
			Assert.AreEqual("12+3*4", expression.ToString());
		}

		[TestMethod]
		public void SinExpression()
		{
			var expression = Expression.ParseExpression("sin(3)");
			Assert.AreEqual("sin(3)", expression.ToString());
		}
	}
}
