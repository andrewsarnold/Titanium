using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;
using Titanium.Core.Functions;
using Titanium.Core.Reducer;
using Titanium.Core.Tokens;

namespace Titanium.Core.Expressions
{
	internal static class ExpressionParser
	{
		internal static Expression Parse(string input)
		{
			var tokens = Tokenizer.Tokenize(input);
			var postfix = CreatePostfixTokenList(tokens);
			return ParsePostfix(postfix);
		}

		private static IEnumerable<Token> CreatePostfixTokenList(List<Token> tokens)
		{
			tokens = InsertImpliedMultiplication(tokens);

			// https://en.wikipedia.org/wiki/Shunting-yard_algorithm

			var outputQueue = new List<Token>();
			var stack = new Stack<Token>();

			for (var index = 0; index < tokens.Count; index++)
			{
				var currentToken = tokens[index];

				// If the token is a number, then add it to the output queue.
				if (currentToken.Type.IsOperand())
				{
					outputQueue.Add(currentToken);
				}

				// If the token is a function token, then push it onto the stack.
				else if (currentToken.Type == TokenType.Function)
				{
					stack.Push(currentToken);
				}

				// If the token is a function argument separator (e.g., a comma):
				else if (currentToken.Type == TokenType.Comma)
				{
					// Until the token at the top of the stack is a left parenthesis,
					// pop operators off the stack onto the output queue.
					while (stack.Peek().Type != TokenType.OpenParenthesis)
					{
						outputQueue.Add(stack.Pop());
					}

					// If no left parentheses are encountered, either the separator
					// was misplaced or parentheses were mismatched.
				}

				// If the token is an operator, o1, then:
				else if (currentToken.Type.IsOperator())
				{
					// while there is an operator token o2, or a function token fun,
					// at the top of the operator stack:
					while (stack.Any() && (stack.Peek().Type.IsOperator() || stack.Peek().Type == TokenType.Function))
					{
						var somethingChanged = false;

						// if it is a function token then pop fun off the operator 
						// stack, onto the output queue;
						if (stack.Peek().Type == TokenType.Function)
						{
							outputQueue.Add(stack.Pop());
							somethingChanged = true;
						}

						// if on the other hand it is an operator token, and either
						//    o1 is left-associative and its precedence is less than or equal to that of o2, or
						//    o1 is right associative, and has precedence less than that of o2,
						// then pop o2 off the operator stack, onto the output queue;
						else
						{
							var topType = stack.Peek().Type;
							if (topType.IsOperator())
							{
								var o1Associativity = currentToken.Type.Associativity();
								var o1Precedence = currentToken.Type.Precedence();
								var o2Precedence = topType.Precedence();

								if ((o1Associativity == OperatorAssociativity.Left && o1Precedence <= o2Precedence) ||
								    (o1Associativity == OperatorAssociativity.Right && o1Precedence < o2Precedence))
								{
									outputQueue.Add(stack.Pop());
									somethingChanged = true;
								}
							}
						}

						if (!somethingChanged) break;
					}

					// at the end of iteration push o1 onto the operator stack.
					stack.Push(currentToken);
				}

				// If the token is a left parenthesis (i.e. "("), then push it onto the stack.
				else if (currentToken.Type == TokenType.OpenParenthesis)
				{
					stack.Push(currentToken);
				}

				// If the token is a right parenthesis (i.e. ")"):
				else if (currentToken.Type == TokenType.CloseParenthesis)
				{
					// Until the token at the top of the stack is a left parenthesis,
					// pop operators off the stack onto the output queue.
					while (stack.Any() && stack.Peek().Type != TokenType.OpenParenthesis)
					{
						outputQueue.Add(stack.Pop());
					}

					// Pop the left parenthesis from the stack, but not onto the output queue.
					stack.Pop();

					// If the token at the top of the stack is a function token, pop it onto the output queue.
					if (stack.Any() && stack.Peek().Type == TokenType.Letter)
					{
						outputQueue.Add(stack.Pop());
					}

					// If the stack runs out without finding a left parenthesis,
					// then there are mismatched parentheses.
				}

				else if (currentToken.Type == TokenType.OpenBrace)
				{
					var indexOfCloseBrace = tokens.FindLastIndex(t => t.Type == TokenType.CloseBrace);
					var tokenSubstring = tokens.Skip(index + 1).Take(indexOfCloseBrace - index - 1).ToList();

					if (tokenSubstring.Count > 0)
					{
						var operands = ParseCommaSeparatedList(tokenSubstring).ToList();
						var list = new ExpressionListToken(new ExpressionList(operands));
						outputQueue.Add(list);
					}
					else
					{	// Special case for empty lists
						outputQueue.Add(new ExpressionListToken(new ExpressionList(new List<Expression>())));
					}

					index = indexOfCloseBrace;
				}

				else if (currentToken.Type == TokenType.CloseBrace)
				{
					throw new SyntaxErrorException("Mismatched braces");
				}

				else
				{
					throw new SyntaxErrorException("Unexpected token", currentToken.Value);
				}
			}

			// When there are no more tokens to read:
			// While there are still operator tokens in the stack:
			while (stack.Any())
			{
				// If the operator token on the top of the stack is a parenthesis,
				// then there are mismatched parentheses.
				if (stack.Peek().Type == TokenType.OpenParenthesis || stack.Peek().Type == TokenType.CloseParenthesis)
				{
					throw new SyntaxErrorException("Mismatched parentheses");
				}

				// Pop the operator onto the output queue.
				outputQueue.Add(stack.Pop());
			}

			return outputQueue;
		}

		private static List<Token> InsertImpliedMultiplication(List<Token> tokens)
		{
			var insertPoints = new List<int>();
			for (var i = 0; i < tokens.Count - 1; i++)
			{
				if (IsImpliedMultiplication(tokens[i], tokens[i + 1]))
				{
					insertPoints.Add(i + 1);
				}
			}

			insertPoints.Reverse();
			foreach (var point in insertPoints)
			{
				tokens.Insert(point, new Token(TokenType.Multiply, "*"));
			}

			return tokens;
		}

		private static bool IsImpliedMultiplication(Token left, Token right)
		{
			return
				(left.Type.IsOperand() && right.Type == TokenType.OpenParenthesis) ||
				(left.Type == TokenType.CloseParenthesis && right.Type.IsOperand()) ||
				(left.Type == TokenType.CloseParenthesis && right.Type == TokenType.OpenParenthesis) ||
				(left.Type.IsOperand() && right.Type == TokenType.Function && !FunctionRepository.Get(right.Value).IsPostFix) ||
				(left.Type.IsOperand() && right.Type.IsOperand());
		}

		private static Expression ParsePostfix(IEnumerable<Token> tokens)
		{
			var stack = new Stack<IEvaluatable>();
			foreach (var token in tokens)
			{
				if (token.Type.IsOperand())
				{
					if (token is ExpressionListToken)
					{
						stack.Push(((ExpressionListToken)token).List);
					}
					else
					{
						stack.Push(new SingleFactorComponent(ParseOperand(token)));
					}
				}
				else if (token.Type == TokenType.Function)
				{
					var operands = new List<Expression>();
					var operatorCount = FunctionRepository.Get(token.Value).ArgumentCount;
					for (var i = 0; i < operatorCount; i++)
					{
						operands.Add(Expressionizer.ToExpression(stack.Pop()));
					}

					stack.Push(new FunctionComponent(token.Value, operands));
				}
				else if (token.Type.IsOperator())
				{
					IEvaluatable parent;

					if (token.Type == TokenType.Factorial || token.Type == TokenType.Negate)
					{
						var argument = stack.Pop();
						parent = new FunctionComponent(token.Value, new List<Expression> { Expressionizer.ToExpression(argument) });
					}
					else if (token.Type == TokenType.Root)
					{
						var argument = stack.Pop();
						parent = new FunctionComponent("√", new List<Expression> { Expressionizer.ToExpression(argument) });
					}
					else
					{
						var right = stack.Pop();
						var left = stack.Pop();

						switch (token.Type)
						{
							case TokenType.Plus:
							case TokenType.Minus:
								parent = new DualComponentExpression(Componentizer.ToComponent(left), Componentizer.ToComponent(right), token.Type == TokenType.Plus);
								break;
							case TokenType.Multiply:
							case TokenType.Divide:
							case TokenType.Exponent:
								parent = new DualFactorComponent(Factorizer.ToFactor(left), Factorizer.ToFactor(right),
									token.Type == TokenType.Multiply
										? ComponentType.Multiply
										: token.Type == TokenType.Divide
											? ComponentType.Divide
											: ComponentType.Exponent);
								break;
							default:
								throw new SyntaxErrorException("Token {0} not expected", token.Value);
						}
					}

					stack.Push(parent);
				}
			}

			if (stack.Count != 1)
			{
				throw new SyntaxErrorException("Stack not parseable");
			}

			var result = stack.Pop();

			if (result is Expression)
			{
				return (Expression)result;
			}

			if (result is Component)
			{
				return new SingleComponentExpression((Component)result);
			}

			if (result is Factor)
			{
				return new SingleComponentExpression(new SingleFactorComponent((Factor)result));
			}

			throw new SyntaxErrorException("Expression tree not parseable");
		}

		private static Factor ParseOperand(Token token)
		{
			switch (token.Type)
			{
				case TokenType.Integer:
					return Factor.GetIntegerFactor(token);

				case TokenType.Float:
					return Factor.GetFloatFactor(token);

				case TokenType.Letter:
					return new AlphabeticFactor(token.Value);
			}

			throw new SyntaxErrorException("Couldn't parse operand {0}", token.Value);
		}

		private static IEnumerable<Expression> ParseCommaSeparatedList(IReadOnlyList<Token> tokens)
		{
			var currentElement = new List<Token>();
			var tokenGroups = new List<List<Token>>();

			for (var i = 0; i < tokens.Count; i++)
			{
				var token = tokens[i];
				if (token.Type == TokenType.Comma)
				{
					tokenGroups.Add(currentElement.Select(e => e).ToList()); // move a copy of the list, not a reference
					currentElement.Clear();
				}

				else if (token.Type == TokenType.OpenBrace)
				{
					var indexOfMatchingBrace = IndexOfMatchingBrace(tokens, i);
					var tokenSubList = tokens.Skip(i + 1).Take(indexOfMatchingBrace - i - 1).ToList();
					var innerList = ParseCommaSeparatedList(tokenSubList).ToList();
					var list = new ExpressionListToken(new ExpressionList(innerList));
					currentElement.Add(list);
					i = indexOfMatchingBrace;
				}

				else if (token.Type == TokenType.CloseBrace)
				{
					throw new SyntaxErrorException("Mismatched braces");
				}

				else
				{
					currentElement.Add(token);
				}
			}

			tokenGroups.Add(currentElement.Select(e => e).ToList()); // move a copy of the list
			currentElement.Clear();

			return tokenGroups.Select(tokenGroup => ParsePostfix(CreatePostfixTokenList(tokenGroup)));
		}

		private static int IndexOfMatchingBrace(IReadOnlyList<Token> tokens, int indexOfOpeningBrace)
		{
			var depth = 0;
			for (var i = indexOfOpeningBrace + 1; i < tokens.Count; i++)
			{
				if (tokens[i].Type == TokenType.OpenBrace)
				{
					depth++;
				}

				if (tokens[i].Type == TokenType.CloseBrace)
				{
					if (depth > 0)
					{
						depth--;
					}
					else
					{
						return i;
					}
				}
			}

			throw new SyntaxErrorException("Mismatched braces");
		}
	}
}
