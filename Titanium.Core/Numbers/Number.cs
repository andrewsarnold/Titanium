using System;
using Titanium.Core.Components;
using Titanium.Core.Exceptions;
using Titanium.Core.Factors;

namespace Titanium.Core.Numbers
{
	internal abstract class Number : IEquatable<Number>
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
			if (right.IsNegative)
			{
				throw new NonRealResultException();
			}

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

		public static Component operator *(IntegerFraction left, Number right)
		{
			if (right is Float)
			{
				return new SingleFactorComponent(new NumericFactor(left * (Float)right));
			}

			return left * (Integer)right;
		}

		public static Component operator /(IntegerFraction left, Number right)
		{
			if (right is Float)
			{
				return new SingleFactorComponent(new NumericFactor(left / (Float)right));
			}

			return left / (Integer)right;
		}

		public static Component operator ^(IntegerFraction left, Number right)
		{
			if (right.IsNegative)
			{
				throw new NonRealResultException();
			}

			if (right is Float)
			{
				return new SingleFactorComponent(new NumericFactor(left ^ (Float)right));
			}

			return left ^ (Integer)right;
		}

		public static Component operator *(Number left, IntegerFraction right)
		{
			if (left is Float)
			{
				return new SingleFactorComponent(new NumericFactor((Float)left * right));
			}

			return (Integer)left * right;
		}

		public static Component operator /(Number left, IntegerFraction right)
		{
			if (left is Float)
			{
				return new SingleFactorComponent(new NumericFactor((Float)left / right));
			}

			return (Integer)left / right;
		}

		public static Component operator ^(Number left, IntegerFraction right)
		{
			if (right.IsNegative)
			{
				throw new NonRealResultException();
			}

			if (left is Float)
			{
				return new SingleFactorComponent(new NumericFactor((Float)left ^ right));
			}

			return ((Integer)left) ^ right;
		}

		internal static bool IsWholeNumber(Number n)
		{
			if (n is Integer) return true;
			return Float.IsWholeNumber((Float)n);
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

		internal abstract double ValueAsFloat();
		internal abstract bool IsNegative { get; }
		internal abstract bool IsZero { get; }
		internal abstract bool IsOne { get; }
		public abstract bool Equals(Number other);
	}
}
