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
				case TokenType.Assign:
					return 7;
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
				case TokenType.Assign:
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
				case TokenType.Assign:
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
				case TokenType.String:
					return true;
			}

			return false;
		}
	}
}
