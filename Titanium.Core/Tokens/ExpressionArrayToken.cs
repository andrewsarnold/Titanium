using Titanium.Core.Factors;

namespace Titanium.Core.Tokens
{
	internal class ExpressionArrayToken : Token
	{
		internal readonly ExpressionArray Array;

		internal ExpressionArrayToken(ExpressionArray array)
			: base(TokenType.ExpressionArray, string.Empty)
		{
			Array = array;
		}
	}
}
