using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Titanium.Test.EvaluateTests
{
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
			Common.EvaluateAndAssert("π*2", "π*2");
		}

		[TestMethod]
		public void ETimesIntegerIsNotExpanded()
		{
			Common.EvaluateAndAssert("e*2", "e*2");
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
	}
}