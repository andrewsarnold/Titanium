using Titanium.Core.Exceptions;

namespace Titanium.Core.Tokens
{
	public static class OperatorExtensions
	{
		public static int Precedence(this TokenType type)
		{
			switch (type)
			{
				case TokenType.Plus:
				case TokenType.Minus:
					return 2;
				case TokenType.Multiply:
				case TokenType.Divide:
					return 3;
				case TokenType.Exponent:
					return 4;
				case TokenType.Factorial:
					return 5;
			}

			throw new UnexpectedTokenTypeException(type);
		}

		public static OperatorAssociativity Associativity(this TokenType type)
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
					return OperatorAssociativity.Irrelevant;
			}

			throw new UnexpectedTokenTypeException(type);
		}

		public static bool IsOperator(this TokenType type)
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
					return true;
			}

			return false;
		}

		public static bool IsOperand(this TokenType type)
		{
			switch (type)
			{
				case TokenType.Integer:
				case TokenType.Float:
				case TokenType.Letter:
					return true;
			}

			return false;
		}
	}
}
