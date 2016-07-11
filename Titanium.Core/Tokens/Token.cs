namespace Titanium.Core.Tokens
{
	internal class Token
	{
		internal TokenType Type { get; private set; }
		internal string Value { get; private set; }

		internal Token(TokenType type, string value)
		{
			Type = type;
			Value = value;
		}

		public override string ToString()
		{
			return string.Format(" {0} ", Value);
		}
	}
}
