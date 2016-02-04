using Titanium.Core.Factors;

namespace Titanium.Core.Tokens
{
	internal class ExpressionListToken : Token
	{
		internal readonly ExpressionList List;

		public ExpressionListToken(ExpressionList list)
			: base(TokenType.ExpressionList, string.Empty)
		{
			List = list;
		}
	}
}
