using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Titanium.Core.Tokens
{
	internal static class Tokenizer
	{
		private static readonly Dictionary<TokenType, Regex> TokenDefinitions = new Dictionary<TokenType, Regex>
		{
			{ TokenType.Number, new Regex(@"\d") },
			{ TokenType.Letter, new Regex(@"[a-zA-ZΑ-ω_]") },
			{ TokenType.OpenParenthesis, new Regex(@"\(") },
			{ TokenType.CloseParenthesis, new Regex(@"\)") },
			{ TokenType.Period, new Regex(@"\.") },
			{ TokenType.Comma, new Regex(@",") },
			{ TokenType.Plus, new Regex(@"\+") },
			{ TokenType.Minus, new Regex(@"-") },
			{ TokenType.Negative, new Regex(@"⁻") },
			{ TokenType.Multiply, new Regex(@"\*") },
			{ TokenType.Divide, new Regex(@"\/") },
			{ TokenType.Exponent, new Regex(@"\^") },
			{ TokenType.Factorial, new Regex(@"!") },
			{ TokenType.None, null }
		};

		private static readonly List<string> BuiltInFunctions = new List<string>
		{
			"sin",
			"cos",
			"tan"
		};

		private static readonly Dictionary<TokenType, bool> Combinables = new Dictionary<TokenType, bool>
		{
			{ TokenType.Number, true },
			{ TokenType.Letter, true }
		};

		internal static List<Token> Tokenize(string input)
		{
			var tokens = new List<Token>();
			var currentTokenType = TokenType.None;
			var currentToken = string.Empty;

			while (input.Contains("⁻⁻"))
			{
				input = input.Replace("⁻⁻", "");
			}

			for (var i = 0; i < input.Length; i++)
			{
				var testCharacter = input.Substring(i, 1);

				if (string.IsNullOrWhiteSpace(testCharacter))
				{
					continue;
				}

				var result = TokenDefinitions.FirstOrDefault(d => d.Value.IsMatch(testCharacter));
				var type = result.Key;

				if (Combinables.ContainsKey(type) && currentTokenType == type)
				{
					currentToken += testCharacter;
				}
				else
				{
					if (!string.IsNullOrEmpty(currentToken))
					{
						tokens.Add(new Token(currentTokenType, currentToken));
					}
					currentToken = testCharacter;
					currentTokenType = type;
				}
			}

			if (!string.IsNullOrEmpty(currentToken))
			{
				tokens.Add(new Token(currentTokenType, currentToken));
			}

			return ConvertFunctions(ConsolidateNumerics(tokens)).ToList();
		}

		internal static List<Token> ConsolidateNumerics(IEnumerable<Token> tokens)
		{
			// Take loose tokens and glue numbers together.

			var returnValue = new List<Token>();

			var numericTokenValue = string.Empty;

			foreach (var t in tokens)
			{
				if (IsNumberComponent(t.Type))
				{
					numericTokenValue += t.Value;
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(numericTokenValue))
					{
						returnValue.Add(new Token(TokenType.Number, numericTokenValue));
					}

					numericTokenValue = string.Empty;
					returnValue.Add(t);
				}
			}

			if (!string.IsNullOrWhiteSpace(numericTokenValue))
			{
				returnValue.Add(new Token(TokenType.Number, numericTokenValue));
			}

			return returnValue;
		}

		private static IEnumerable<Token> ConvertFunctions(IEnumerable<Token> tokens)
		{
			return tokens.Select(token => token.Type == TokenType.Letter && BuiltInFunctions.Contains(token.Value)
				? new Token(TokenType.Function, token.Value)
				: token);
		}

		private static bool IsNumberComponent(TokenType type)
		{
			return new[] { TokenType.Negative, TokenType.Number, TokenType.Period }.Contains(type);
		}
	}
}
