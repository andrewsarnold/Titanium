using System.Collections.Generic;
using System.Linq;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Functions;
using Titanium.Core.Functions.Implementations;

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
			if (Function is Negate)
			{
				return 1;
			}

			var other = obj as FunctionComponent;
			if (other != null)
			{
				return 0;
			}

			var sfc = obj as SingleFactorComponent;
			if (sfc != null)
			{
				return -1;
			}

			var cl = obj as ComponentList;
			if (cl != null)
			{
				return 0;
			}

			throw new IncomparableTypeException(GetType(), obj.GetType());
		}

		public override bool Equals(Evaluatable other)
		{
			var fc = other as FunctionComponent;
			if (fc != null)
			{
				return Function.Name == fc.Function.Name &&
					   !Operands.Where((t, i) => !t.Equals(fc.Operands[i])).Any();
			}

			return false;
		}

		public override string ToString()
		{
			return Function.ToString(Operands);
		}
	}
}
