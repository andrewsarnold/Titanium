using System.Collections.Generic;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;
using Titanium.Core.Functions;
using Titanium.Core.Numbers;

namespace Titanium.Core.Components
{
	internal class FunctionComponent : Component
	{
		private readonly string _functionName;
		private readonly List<object> _operands;

		public FunctionComponent(string functionName, List<object> operands)
		{
			_functionName = functionName;
			_operands = operands;
		}

		internal override Component Evaluate()
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
