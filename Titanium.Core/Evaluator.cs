using System.Collections.Generic;
using Titanium.Core.Expressions;

namespace Titanium.Core
{
	public class Evaluator
	{
		private readonly Stack<Expression> _history;

		public Evaluator()
		{
			_history = new Stack<Expression>();
		}

		public string Evaluate(string input)
		{
			var result = Expression.ParseExpression(input).Evaluate();
			_history.Push(result);
			return result.ToString();
		}
	}
}
