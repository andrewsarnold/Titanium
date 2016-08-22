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
		public void CantAssignToString()
		{
			Common.AssertThrows<InvalidVariableOrFunctionNameException>("5→\"xyz\"");
		}

		[TestMethod]
		public void CantAssignToExpression()
		{
			Common.AssertThrows<InvalidVariableOrFunctionNameException>("5→(x+2)");
		}

		[TestMethod]
		public void AssignedVariableEvaluatesToSetValue()
		{
			_evaluator.Evaluate("5→x");
			Common.EvaluateAndAssert(_evaluator, "x", "5");
		}

		[TestMethod]
		public void ReassignedVariableIsOverwritten()
		{
			_evaluator.Evaluate("5→x");
			_evaluator.Evaluate("6→x");
			Common.EvaluateAndAssert(_evaluator, "x", "6");
		}

		[TestMethod]
		public void AssigningToSelfDoesNothing()
		{
			_evaluator.Evaluate("5→x");
			_evaluator.Evaluate("x→x");
			Common.EvaluateAndAssert(_evaluator, "x", "5");
		}

		[TestMethod]
		public void AssignSyntaxError()
		{
			Common.AssertThrows<SyntaxErrorException>("5→^");
		}

		[TestMethod]
		public void AssignmentIsEvaluated()
		{
			Common.EvaluateAndAssert("2+3→x", "5");
		}

		[TestMethod]
		public void BasicUnassignment()
		{
			_evaluator.Evaluate("5→x");
			Common.EvaluateAndAssert(_evaluator, "x", "5");
			Common.EvaluateAndAssert(_evaluator, "DelVar x", "Done");
			Common.EvaluateAndAssert(_evaluator, "x", "x");
		}

		[TestMethod]
		public void UnassignUndefinedStillWorks()
		{
			Common.EvaluateAndAssert("DelVar xyz", "Done");
		}

		[TestMethod]
		public void UnassignNothingError()
		{
			Common.AssertThrows<SyntaxErrorException>("DelVar");
		}

		[TestMethod]
		public void UnassignSpaceError()
		{
			Common.AssertThrows<SyntaxErrorException>("DelVar ");
		}

		[TestMethod]
		public void UnassignNonVariableError()
		{
			Common.AssertThrows<ArgumentMustBeAVariableNameException>("DelVar (5+x)");
		}

		[TestMethod]
		public void UnassignMultiple()
		{
			_evaluator.Evaluate("3→x");
			_evaluator.Evaluate("4→y");
			Common.EvaluateAndAssert(_evaluator, "x", "3");
			Common.EvaluateAndAssert(_evaluator, "y", "4");
			Common.EvaluateAndAssert(_evaluator, "DelVar x,y", "Done");
			Common.EvaluateAndAssert(_evaluator, "x", "x");
			Common.EvaluateAndAssert(_evaluator, "y", "y");
		}
	}
}
