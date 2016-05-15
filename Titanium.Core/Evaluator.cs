﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Titanium.Core.Expressions;

namespace Titanium.Core
{
	public class Evaluator
	{
		private readonly List<Expression> _history;

		public Evaluator()
		{
			_history = new List<Expression>();
		}

		public string Evaluate(string input)
		{
			input = ReplaceResultHistory(input);
			var result = Expression.ParseExpression(input).Evaluate();
			_history.Insert(0, result);
			return result.ToString();
		}

		private string ReplaceResultHistory(string input)
		{
			var matches = Regex.Matches(input, @"ans\((\d+)\)");

			foreach (var match in matches.Cast<Match>())
			{
				var index = int.Parse(match.Groups[1].Value) - 1;
				var result = string.Format("({0})", _history[index]);
				input = input.Replace(match.Value, result);
			}

			return input;
		}
	}
}
