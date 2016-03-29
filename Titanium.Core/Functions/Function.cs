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
		internal readonly FixType FixType;

		protected Function(string name, int argumentCount, FixType fixType = FixType.PostFix)
		{
			FixType = fixType;
			Name = name;
			ArgumentCount = argumentCount;
		}

		internal Expression Evaluate(params Expression[] parameters)
		{
			if (parameters.Length != ArgumentCount)
			{
				throw new WrongArgumentCountException(Name, ArgumentCount, parameters.Length);
			}

			return InnerEvaluate(parameters);
		}

		protected Expression AsExpression(params Expression[] parameters)
		{
			return Expressionizer.ToExpression(new FunctionComponent(this, new List<Expression>(parameters)));
		}

		protected abstract Expression InnerEvaluate(params Expression[] parameters);
		internal abstract string ToString(List<Expression> parameters);
	}
}
