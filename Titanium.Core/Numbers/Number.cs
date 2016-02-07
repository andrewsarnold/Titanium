using System;
using Titanium.Core.Components;
using Titanium.Core.Factors;

namespace Titanium.Core.Numbers
{
	public abstract class Number
	{
		public static Number operator +(Number left, Number right)
		{
			return Evaluate(left, right, (x, y) => x + y);
		}

		public static Number operator -(Number left, Number right)
		{
			return Evaluate(left, right, (x, y) => x - y);
		}

		public static Number operator ^(Number left, Number right)
		{
			return Evaluate(left, right, Math.Pow);
		}

		public static Number operator *(Number left, Number right)
		{
			return Evaluate(left, right, (x, y) => x * y);
		}

		public static object operator /(Number left, Number right)
		{
			if (Math.Abs(right.ValueAsFloat()) < Constants.Tolerance) throw new DivideByZeroException();

			if (left is Integer && right is Integer)
			{
				return new IntegerFraction((Integer)left, (Integer)right);
			}

			return Evaluate(left, right, (x, y) => x / y);
		}

		private static Number Evaluate(Number left, Number right, Func<double, double, double> operation)
		{
			var leftInteger = left as Integer;
			var rightInteger = right as Integer;

			if (leftInteger != null && rightInteger != null)
			{
				return new Integer((int)operation(leftInteger.Value, rightInteger.Value));
			}

			return new Float(operation(left.ValueAsFloat(), right.ValueAsFloat()));
		}

		protected abstract double ValueAsFloat();
		internal abstract bool IsNegative { get; }
	}
}
