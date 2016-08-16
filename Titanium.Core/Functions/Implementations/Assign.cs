using System.Collections.Generic;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Reducer;

namespace Titanium.Core.Functions.Implementations
{
	internal class Assign : Function
	{
		private readonly Dictionary<string, Expression> _variableMap;

		public Assign(Dictionary<string, Expression> variableMap)
			: base("→", 2, FixType.PreFix)
		{
			_variableMap = variableMap;
		}

		protected override Expression InnerEvaluate(params Expression[] parameters)
		{
			var variable = Factorizer.ToFactor(parameters[1]);
			if (!(variable is AlphabeticFactor))
			{
				throw new InvalidVariableOrFunctionNameException();
			}

			var key = ((AlphabeticFactor)variable).Value;
			if (_variableMap.ContainsKey(key))
			{
				_variableMap[key] = parameters[0].Evaluate();
			}
			else
			{
				_variableMap.Add(key, parameters[0]);
			}

			return parameters[0];
		}

		internal override string ToString(List<Expression> parameters)
		{
			return parameters[0].ToString();
		}
	}
}
