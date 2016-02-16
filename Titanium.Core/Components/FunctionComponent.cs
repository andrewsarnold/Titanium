using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Functions;

namespace Titanium.Core.Components
{
	internal class FunctionComponent : Component
	{
		internal readonly Function Function;
		internal readonly List<Expression> Operands;

		public FunctionComponent(string name, List<Expression> operands)
		{
			Function = FunctionRepository.Get(name);
			Operands = operands;
		}

		public FunctionComponent(Function function, List<Expression> operands)
		{
			Function = function;
			Operands = operands;
		}

		public override Expression Evaluate()
		{
			return Function.Evaluate(Operands.ToArray());
		}
		
		public override string ToString()
		{
			return Function.ToString(Operands);
		}
	}
}
