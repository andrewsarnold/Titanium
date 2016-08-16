using Titanium.Core.Exceptions;

namespace Titanium.Core.Expressions
{
	internal sealed class UndefinedExpression : Expression
	{
		internal override Expression Evaluate()
		{
			return this;
		}

		public override int CompareTo(object obj)
		{
			var other = obj as UndefinedExpression;
			if (other != null)
			{
				return 0;
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			return other is UndefinedExpression;
		}

		public override string ToString()
		{
			return "undef";
		}
	}
}
