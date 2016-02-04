using System.Collections.Generic;
using Titanium.Core.Expressions;
using Titanium.Core.Functions;

namespace Titanium.Core.Components
{
	internal class FunctionComponent : Component
	{
		private readonly string _functionName;
		private readonly List<IEvaluatable> _operands;

		public FunctionComponent(string functionName, List<IEvaluatable> operands)
		{
			_functionName = functionName;
			_operands = operands;
		}

		public override Expression Evaluate()
		{
			return FunctionRepository.Evaluate(_functionName, _operands);
		}
		
		public override string ToString()
		{
			// Special formats
			if (_functionName == "!")
			{
				return string.Format("({0})!", _operands[0]);
			}

			return string.Format("{0}({1})", _functionName, string.Join(",", _operands));
		}
	}
}
