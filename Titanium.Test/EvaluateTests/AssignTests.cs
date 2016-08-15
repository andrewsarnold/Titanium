using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core;
using Titanium.Core.Exceptions;

namespace Titanium.Test.EvaluateTests
{
	[TestClass]
	public class AssignTests
	{
		private Evaluator _evaluator;

		[TestInitialize]
		public void Initialize()
		{
			_evaluator = new Evaluator();
		}

		[TestMethod]
		public void AllowAssigning()
		{
			Common.EvaluateAndAssert("5→x", "5");
		}

		[TestMethod]
		public void AllowAssigningWithParentheses()
		{
			Common.EvaluateAndAssert("5→(x)", "5");
		}

		[TestMethod]
		public void CantAssignToNumber()
		{
			Common.AssertThrows<InvalidVariableOrFunctionNameException>("5→6");
		}

		[TestMethod]
		public void AssignedVariableEvaluatesToSetValue()
		{
			_evaluator.Evaluate("5→x");
			Common.EvaluateAndAssert(_evaluator, "x", "5");
		}
	}
}
