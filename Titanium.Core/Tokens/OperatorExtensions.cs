using Titanium.Core.Exceptions;

namespace Titanium.Core.Tokens
{
	internal static class OperatorExtensions
	{
		internal static int Precedence(this TokenType type)
		{
			switch (type)
			{
				case TokenType.Plus:
				case TokenType.Minus:
					return 2;
				case TokenType.Multiply:
				case TokenType.Divide:
					return 3;
				case TokenType.Root:
					return 4;
				case TokenType.Exponent:
					return 5;
				case TokenType.Factorial:
					return 6;
			}

			throw new UnexpectedTokenTypeException(type);
		}

		internal static OperatorAssociativity Associativity(this TokenType type)
		{
			switch (type)
			{
				case TokenType.Plus:
				case TokenType.Minus:
				case TokenType.Multiply:
				case TokenType.Divide:
					return OperatorAssociativity.Left;
				case TokenType.Exponent:
					return OperatorAssociativity.Right;
				case TokenType.Factorial:
				case TokenType.Root:
					return OperatorAssociativity.Irrelevant;
			}

			throw new UnexpectedTokenTypeException(type);
		}

		internal static bool IsOperator(this TokenType type)
		{
			switch (type)
			{
				case TokenType.Plus:
				case TokenType.Minus:
				case TokenType.Multiply:
				case TokenType.Divide:
				case TokenType.Exponent:
				case TokenType.Function:
				case TokenType.Factorial:
				case TokenType.Root:
					return true;
			}

			return false;
		}

		internal static bool IsOperand(this TokenType type)
		{
			switch (type)
			{
				case TokenType.Integer:
				case TokenType.Float:
				case TokenType.Letter:
				case TokenType.ExpressionList:
					return true;
			}

			return false;
		}
	}
}
