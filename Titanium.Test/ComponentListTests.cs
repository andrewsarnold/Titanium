using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Components;
using Titanium.Core.Factors;
using Titanium.Core.Reducer;

namespace Titanium.Test
{
	[TestClass]
	public class ComponentListTests
	{
		[TestMethod]
		public void LeftNestedComponentsAreInlined()
		{
			// (x*y)*z = x*y*z
			var innerComponent = new DualFactorComponent(new AlphabeticFactor("x"), new AlphabeticFactor("y"), true);
			var outerComponent = new DualFactorComponent(Factorizer.ToFactor(innerComponent), new AlphabeticFactor("z"), true);
			var componentList = new ComponentList(outerComponent);

			Assert.AreEqual(3, componentList.Factors.Count);
			Assert.IsTrue(componentList.Factors[0].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[0].IsMultiply);
			Assert.IsTrue(componentList.Factors[1].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[1].IsMultiply);
			Assert.IsTrue(componentList.Factors[2].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[2].IsMultiply);

			Assert.AreEqual("x*y*z", componentList.ToString());
		}

		[TestMethod]
		public void RightNestedComponentsAreInlined()
		{
			// x*(y*z) = x*y*z
			var innerComponent = new DualFactorComponent(new AlphabeticFactor("y"), new AlphabeticFactor("z"), true);
			var outerComponent = new DualFactorComponent(new AlphabeticFactor("x"), Factorizer.ToFactor(innerComponent), true);
			var componentList = new ComponentList(outerComponent);

			Assert.AreEqual(3, componentList.Factors.Count);
			Assert.IsTrue(componentList.Factors[0].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[0].IsMultiply);
			Assert.IsTrue(componentList.Factors[1].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[1].IsMultiply);
			Assert.IsTrue(componentList.Factors[2].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[2].IsMultiply);

			Assert.AreEqual("x*y*z", componentList.ToString());
		}

		[TestMethod]
		public void DoubleNestedComponentsAreInlined()
		{
			// (w*x)*(y*z) = w*x*y*z
			var leftInnerComponent = new DualFactorComponent(new AlphabeticFactor("w"), new AlphabeticFactor("x"), true);
			var rightInnerComponent = new DualFactorComponent(new AlphabeticFactor("y"), new AlphabeticFactor("z"), true);
			var outerComponent = new DualFactorComponent(Factorizer.ToFactor(leftInnerComponent), Factorizer.ToFactor(rightInnerComponent), true);
			var componentList = new ComponentList(outerComponent);

			Assert.AreEqual(4, componentList.Factors.Count);
			Assert.IsTrue(componentList.Factors[0].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[0].IsMultiply);
			Assert.IsTrue(componentList.Factors[1].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[1].IsMultiply);
			Assert.IsTrue(componentList.Factors[2].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[2].IsMultiply);
			Assert.IsTrue(componentList.Factors[3].Factor is AlphabeticFactor);
			Assert.IsTrue(componentList.Factors[3].IsMultiply);

			Assert.AreEqual("w*x*y*z", componentList.ToString());
		}
	}
}
