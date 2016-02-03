namespace Titanium.Core.Expressions
{
	internal sealed class UndefinedExpression : Expression
	{
		public override Expression Evaluate()
		{
			return this;
		}

		public override string ToString()
		{
			return "undef";
		}
	}
}
