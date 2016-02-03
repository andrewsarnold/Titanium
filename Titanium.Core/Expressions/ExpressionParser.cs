using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;
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

		private static IEnumerable<Token> CreatePostfixTokenList(IEnumerable<Token> tokens)
		{
			// https://en.wikipedia.org/wiki/Shunting-yard_algorithm

			var outputQueue = new List<Token>();
			var stack = new Stack<Token>();

			foreach (var currentToken in tokens)
			{
				// If the token is a number, then add it to the output queue.
				if (currentToken.Type == TokenType.Integer || currentToken.Type == TokenType.Float || currentToken.Type == TokenType.Letter)
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

		private static Expression ParsePostfix(IEnumerable<Token> tokens)
		{
			var stack = new Stack<object>();
			foreach (var token in tokens)
			{
				if (token.Type.IsOperand())
				{
					stack.Push(ParseOperand(token));
				}
				else if (token.Type == TokenType.Function)
				{
					var operands = stack.Reverse().ToList();
					stack.Clear();
					stack.Push(new FunctionComponent(token.Value, operands));
				}
				else if (token.Type.IsOperator())
				{
					object parent;

					if (token.Type == TokenType.Factorial)
					{
						var argument = stack.Pop();
						parent = new FunctionComponent("!", new List<object> { argument });
					}
					else
					{
						var right = stack.Pop();
						var left = stack.Pop();

						switch (token.Type)
						{
							case TokenType.Plus:
							case TokenType.Minus:
								parent = new DualComponentExpression(Reducer.GetComponent(left), Reducer.GetComponent(right), token.Type == TokenType.Plus);
								break;
							case TokenType.Multiply:
							case TokenType.Divide:
							case TokenType.Exponent:
								parent = new DualFactorComponent(Reducer.GetFactor(left), Reducer.GetFactor(right),
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
	}
}
