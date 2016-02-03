using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Titanium.Core.Exceptions;
using Titanium.Core.Functions;

namespace Titanium.Core.Tokens
{
	internal static class Tokenizer
	{
		private static readonly Dictionary<TokenType, Regex> TokenDefinitions = new Dictionary<TokenType, Regex>
		{
			{ TokenType.Integer, new Regex(@"^⁻?\d+$") },
			{ TokenType.Float, new Regex(@"^(⁻?\d*\.\d+|⁻?\d+\.\d*)$") },
			{ TokenType.Letter, new Regex(@"^[a-zA-ZΑ-ώ_]+[a-zA-ZΑ-ώ_\d]*$") },
			{ TokenType.OpenParenthesis, new Regex(@"^\($") },
			{ TokenType.CloseParenthesis, new Regex(@"^\)$") },
			{ TokenType.Period, new Regex(@"^\.$") },
			{ TokenType.Comma, new Regex(@"^,$") },
			{ TokenType.Plus, new Regex(@"^\+$") },
			{ TokenType.Minus, new Regex(@"^-$") },
			{ TokenType.Multiply, new Regex(@"^\*$") },
			{ TokenType.Divide, new Regex(@"^\/$") },
			{ TokenType.Exponent, new Regex(@"^\^$") },
			{ TokenType.Factorial, new Regex(@"^!$") },
			{ TokenType.Space, new Regex(@"^\s+$") },
			{ TokenType.None, new Regex("^$") }
		};

		internal static List<Token> Tokenize(string input)
		{
			input = CleanNegatives(input);
			var tokens = new List<Token>();
			var startIndex = 0;

			while (startIndex < input.Length)
			{
				var token = FindNextToken(input.Substring(startIndex));
				if (token.Type != TokenType.None)
				{
					tokens.Add(token);
					startIndex += token.Value.Length;
				}
				else
				{
					throw new Exception("Couldn't get a token");
				}
			}

			return ConvertFunctions(tokens).Where(t => t.Type != TokenType.Space).ToList();
		}

		private static string CleanNegatives(string input)
		{
			while (input.Contains("⁻⁻"))
			{
				input = input.Replace("⁻⁻", "");
			}

			return input;
		}

		private static Token FindNextToken(string input)
		{
			var length = input.Length;

			while (length > 0)
			{
				var testString = input.Substring(0, length);
				var type = TokenDefinitions.FirstOrDefault(t => t.Value.IsMatch(testString)).Key;
				if (type != TokenType.None)
				{
					return new Token(type, testString);
				}

				length--;
			}

			throw new SyntaxErrorException("Couldn't parse a token from string {0}", input);
		}

		private static IEnumerable<Token> ConvertFunctions(IEnumerable<Token> tokens)
		{
			return tokens.Select(token => token.Type == TokenType.Letter && FunctionRepository.Contains(token.Value)
				? new Token(TokenType.Function, token.Value)
				: token);
		}
	}
}
