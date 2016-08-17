using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;

namespace Titanium.Core.Evaluator
{
	public class Evaluator
	{
		private readonly List<Expression> _history;
		private readonly Dictionary<string, Expression> _variableMap; 

		public Evaluator()
		{
			_history = new List<Expression>();
			_variableMap = new Dictionary<string, Expression>();
		}

		public string Evaluate(string input)
		{
			string sysOutput;
			if (EvaluateSystemCommand(input, out sysOutput))
			{
				return sysOutput;
			}
			
			input = ReplaceResultHistory(input);
			var result = Expression.ParseExpression(input, _variableMap).Evaluate();
			_history.Insert(0, result);
			return result.ToString();
		}

		private string ReplaceResultHistory(string input)
		{
			var matches = Regex.Matches(input, @"ans\((\d+)\)");

			foreach (var match in matches.Cast<Match>())
			{
				var index = int.Parse(match.Groups[1].Value) - 1;
				if (index == -1) index = 0; // "ans(0)" returns last answer, just like ans(1)
				if (index > _history.Count - 1) continue; // out of bounds
				
				var result = string.Format("({0})", _history[index]);
				input = input.Replace(match.Value, result);
			}

			return input;
		}

		private bool EvaluateSystemCommand(string input, out string output)
		{
			if (input.StartsWith("DelVar"))
			{
				var delVarMatch = Regex.Match(input, "DelVar(\\s+)([^,]+)(?:,([^,]))*");
				if (delVarMatch.Groups.Count < 3)
				{
					throw new SyntaxErrorException();
				}
				for (var i = 2; i < delVarMatch.Groups.Count; i++)
				{
					var group = delVarMatch.Groups[i];
					if (group.Captures.Count == 0)
					{
						continue;
					}
					var varName = group.Captures[0].Value;
					if (Regex.Matches(varName, "\\W").Count > 0)
					{
						throw new ArgumentMustBeAVariableNameException();
					}
					_variableMap.Remove(varName);
				}
				output = "Done";
				return true;
			}

			output = string.Empty;
			return false;
		}
	}
}
