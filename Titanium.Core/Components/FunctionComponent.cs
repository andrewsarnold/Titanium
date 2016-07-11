using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Exceptions;
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

		public override int CompareTo(object obj)
		{
			var other = obj as FunctionComponent;
			if (other != null)
			{
				
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var fc = other as FunctionComponent;
			if (fc != null)
			{
				return Function.Name == fc.Function.Name &&
					   !fc.Operands.Where((t, i) => !t.Equals(fc.Operands[i])).Any();
			}

			return false;
		}

		public override string ToString()
		{
			return Function.ToString(Operands);
		}
	}
}
