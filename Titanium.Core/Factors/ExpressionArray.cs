using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Factors
{
	internal class ExpressionArray : Factor
	{
		internal List<Expression> Expressions;

		internal ExpressionArray(List<Expression> expressions)
		{
			Expressions = expressions;
		}

		internal override Expression Evaluate(bool expand = false)
		{
			Expressions = Expressions.Select(e => e.Evaluate(expand)).ToList();
			return Expressionizer.ToExpression(this);
		}

		public override string ToString()
		{
			return string.Format("{{{0}}}", string.Join(",", Expressions));
		}

		public override int CompareTo(object obj)
		{
			throw new System.NotImplementedException();
		}

		public override bool Equals(Evaluatable other)
		{
			var obj = other as ExpressionArray;
			if (Expressions.Count != obj?.Expressions.Count)
			{
				return false;
			}

			return !Expressions.Where((t, i) => !t.Equals(obj.Expressions[i])).Any();
		}

		internal override int CompareTo(Factor factor, bool isMultiply)
		{
			throw new System.NotImplementedException();
		}
	}
}
