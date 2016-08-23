using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;

namespace Titanium.Test
{
	[TestClass]
	public class ComparisonTests
	{
		[TestMethod]
		public void EnsureAlphabeticFactorIsBeforeNumericFactor()
		{
			Assert.IsTrue(new AlphabeticFactor("x").CompareTo(new NumericFactor(new Integer(4))) == -1);
		}
	}
}
