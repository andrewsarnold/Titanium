using System;
using Titanium.Core.Expressions;

namespace Titanium.WinConsole
{
	public static class Program
	{
		public static void Main()
		{
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine();
				if (input != null && input.Equals("exit")) break;
				if (string.IsNullOrWhiteSpace(input)) continue;

				try
				{
					Console.WriteLine(CleanOutput(Expression.ParseExpression(input).Evaluate().ToString()).PadLeft(24, ' '));
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

		private static string CleanOutput(string output)
		{
			return output.Replace("⁻", "-");
		}
	}
}
