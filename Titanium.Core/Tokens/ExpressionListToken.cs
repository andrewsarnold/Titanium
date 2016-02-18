using Titanium.Core.Factors;

namespace Titanium.Core.Tokens
{
	internal class ExpressionListToken : Token
	{
		internal readonly ExpressionList List;

		internal ExpressionListToken(ExpressionList list)
			: base(TokenType.ExpressionList, string.Empty)
		{
			List = list;
		}
	}
}
