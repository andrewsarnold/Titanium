using System.Collections.Generic;
using Titanium.Core.Expressions;

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
			var key = parameters[1].ToString();
			if (_variableMap.ContainsKey(key))
			{
				_variableMap[key] = parameters[0];
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
