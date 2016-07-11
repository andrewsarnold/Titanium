using System;
using System.Collections.Generic;
using Titanium.Core;

namespace Titanium.WinConsole
{
	public static class Program
	{
		private static Evaluator _evaluator;

		private static readonly Dictionary<string, Action> SpecialActions = new Dictionary<string, Action>
		{
			{ string.Empty, () => { } },
			{ "exit", () => { Environment.Exit(0); } },
			{ "cls", Console.Clear }
		};

		public static void Main()
		{
			_evaluator = new Evaluator();

			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine() ?? string.Empty;

				if (SpecialActions.ContainsKey(input))
				{
					SpecialActions[input].Invoke();
					continue;
				}

				try
				{
					Console.WriteLine(CleanOutput(_evaluator.Evaluate(input)).PadLeft(24, ' '));
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message.PadLeft(24, ' '));
				}
			}
		}

		private static string CleanOutput(string output)
		{
			return output.Replace("⁻", "-");
		}
	}
}
