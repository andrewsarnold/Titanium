using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Functions;

namespace Titanium.Core.Components
{
	internal class FunctionComponent : Component
	{
		internal readonly Function Function;
		internal readonly List<Expression> Operands;

		internal FunctionComponent(string name, List<Expression> operands)
		{
			Function = FunctionRepository.Get(name);
			Operands = operands;
		}

		internal FunctionComponent(Function function, List<Expression> operands)
		{
			Function = function;
			Operands = operands;
		}

		internal override Expression Evaluate()
		{
			return Function.Evaluate(Operands.ToArray());
		}
		
		public override string ToString()
		{
			return Function.ToString(Operands);
		}
	}
}
