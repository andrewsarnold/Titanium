using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class ConstantTests
	{
		[TestMethod]
		public void PiAloneReturnsItself()
		{
			Common.EvaluateAndAssert("π", "π");
		}

		[TestMethod]
		public void EAloneReturnsItself()
		{
			Common.EvaluateAndAssert("e", "e");
		}

		[TestMethod]
		public void PiTimesIntegerIsNotExpanded()
		{
			Common.EvaluateAndAssert("π*2", "2*π");
		}

		[TestMethod]
		public void ETimesIntegerIsNotExpanded()
		{
			Common.EvaluateAndAssert("e*2", "2*e");
		}

		[TestMethod]
		public void PiTimesFloatIsExpanded()
		{
			Common.EvaluateAndAssert("π*1.", Math.PI.ToString(CultureInfo.InvariantCulture));
		}

		[TestMethod]
		public void ETimesFloatIsExpanded()
		{
			Common.EvaluateAndAssert("e*1.", Math.E.ToString(CultureInfo.InvariantCulture));
		}

		[TestMethod]
		public void ConstantTimesItselfIsSquared()
		{
			Common.EvaluateAndAssert("e*e", "e^2");
		}

		[TestMethod]
		public void ConstantTimesItselfThreeTimesIsCubed()
		{
			Common.EvaluateAndAssert("e*e*e", "e^3");
		}
	}
}
