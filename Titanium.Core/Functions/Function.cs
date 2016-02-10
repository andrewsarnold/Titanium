using System.Collections.Generic;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions
{
	internal abstract class Function
	{
		internal readonly string Name;
		internal readonly int ArgumentCount;
		internal readonly bool IsPostFix;

		protected Function(string name, int argumentCount, bool isPostFix = false)
		{
			IsPostFix = isPostFix;
			Name = name;
			ArgumentCount = argumentCount;
		}

		public Expression Evaluate(List<Expression> parameters)
		{
			if (parameters.Count != ArgumentCount)
			{
				throw new WrongArgumentCountException(Name, ArgumentCount, parameters.Count);
			}

			return InnerEvaluate(parameters);
		}

		protected Expression AsExpression(params Expression[] parameters)
		{
			return Expressionizer.ToExpression(new FunctionComponent(this, new List<Expression>(parameters)));
		}

		protected abstract Expression InnerEvaluate(List<Expression> parameters);
		public abstract string ToString(List<Expression> parameters);
	}
}
