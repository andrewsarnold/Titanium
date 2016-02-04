using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Functions;

namespace Titanium.Core.Components
{
	internal class FunctionComponent : Component
	{
		private readonly Function _function;
		private readonly List<Expression> _operands;

		public FunctionComponent(string name, List<Expression> operands)
		{
			_function = FunctionRepository.Get(name);
			_operands = operands;
		}

		public FunctionComponent(Function function, List<Expression> operands)
		{
			_function = function;
			_operands = operands;
		}

		public override Expression Evaluate()
		{
			return _function.Evaluate(_operands);
		}
		
		public override string ToString()
		{
			return _function.ToString(_operands);
		}
	}
}
