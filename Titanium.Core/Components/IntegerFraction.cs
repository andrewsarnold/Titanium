using System;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	internal class IntegerFraction : Component
	{
		internal readonly bool IsNegative;
		internal readonly int Numerator;
		internal readonly int Denominator;

		internal IntegerFraction(Integer value)
		{
			Numerator = value.Value;
			Denominator = 1;
		}

		internal IntegerFraction(Integer numerator, Integer denominator)
		{
			if (denominator.Value == 0)
			{
				throw new DivideByZeroException();
			}

			var gcd = Integer.GreatestCommonDivisor(numerator, denominator);

			IsNegative = Math.Sign(numerator.Value) * Math.Sign(denominator.Value) < 0;

			Numerator = Math.Abs(numerator.Value / gcd.Value) * (IsNegative ? -1 : 1);
			Denominator = Math.Abs(denominator.Value / gcd.Value);
		}

		public static IntegerFraction operator +(IntegerFraction left, IntegerFraction right)
		{
			var lcm = Math.Abs(left.Denominator * right.Denominator) / Integer.GreatestCommonDivisor(left.Denominator, right.Denominator);

			// calculate new left
			var leftFactor = lcm / left.Denominator;
			var leftNum = left.Numerator * leftFactor;
			var leftDenom = left.Denominator * leftFactor;

			// calculate new right
			var rightFactor = lcm / right.Denominator;
			var rightNum = right.Numerator * rightFactor;
			var rightDenom = right.Denominator * rightFactor;

			if (leftDenom != rightDenom) throw new AlgorithmException("Denominators not the same");

			return new IntegerFraction(new Integer(rightNum + leftNum), new Integer(leftDenom));
		}

		public static IntegerFraction operator -(IntegerFraction left, IntegerFraction right)
		{
			return left + right.Negative();
		}

		public static IntegerFraction operator *(IntegerFraction left, IntegerFraction right)
		{
			return new IntegerFraction(new Integer(left.Numerator * right.Numerator), new Integer(left.Denominator * right.Denominator));
		}

		public static IntegerFraction operator /(IntegerFraction left, IntegerFraction right)
		{
			return new IntegerFraction(new Integer(left.Numerator * right.Denominator), new Integer(left.Denominator * right.Numerator));
		}

		public static IntegerFraction operator ^(IntegerFraction left, IntegerFraction right)
		{
			throw new NotImplementedException();
		}

		public static IntegerFraction operator +(IntegerFraction left, Integer right)
		{
			return left + new IntegerFraction(right, new Integer(1));
		}

		public static IntegerFraction operator -(IntegerFraction left, Integer right)
		{
			return left - new IntegerFraction(right, new Integer(1));
		}

		public static IntegerFraction operator *(IntegerFraction left, Integer right)
		{
			return left * new IntegerFraction(right, new Integer(1));
		}

		public static IntegerFraction operator /(IntegerFraction left, Integer right)
		{
			return left / new IntegerFraction(right, new Integer(1));
		}

		public static IntegerFraction operator ^(IntegerFraction left, Integer right)
		{
			throw new NotImplementedException();
		}

		public static IntegerFraction operator +(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, new Integer(1)) + right;
		}

		public static IntegerFraction operator -(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, new Integer(1)) - right;
		}

		public static IntegerFraction operator *(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, new Integer(1)) * right;
		}

		public static IntegerFraction operator /(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, new Integer(1)) / right;
		}

		public static Number operator +(IntegerFraction left, Float right)
		{
			return left.ValueAsFloat() + right;
		}

		public static Number operator -(IntegerFraction left, Float right)
		{
			return left.ValueAsFloat() - right;
		}

		public static Number operator *(IntegerFraction left, Float right)
		{
			return left.ValueAsFloat() * right;
		}

		public static Number operator /(IntegerFraction left, Float right)
		{
			return (Number)(left.ValueAsFloat() / right);
		}

		public static Number operator ^(IntegerFraction left, Float right)
		{
			return left.ValueAsFloat() ^ right;
		}

		public static Number operator +(Float left, IntegerFraction right)
		{
			return left + right.ValueAsFloat();
		}

		public static Number operator -(Float left, IntegerFraction right)
		{
			return left - right.ValueAsFloat();
		}

		public static Number operator *(Float left, IntegerFraction right)
		{
			return left * right.ValueAsFloat();
		}

		public static Number operator /(Float left, IntegerFraction right)
		{
			return (Number)(left / right.ValueAsFloat());
		}

		public static Component operator +(IntegerFraction left, Number right)
		{
			return right is Integer
				? (Component)(left + ((Integer)right))
				: new SingleFactorComponent(new NumericFactor(left + (Float)right));
		}

		public static Component operator -(IntegerFraction left, Number right)
		{
			return right is Integer
				? (Component)(left - ((Integer)right))
				: new SingleFactorComponent(new NumericFactor(left - (Float)right));
		}

		public static Component operator +(Number left, IntegerFraction right)
		{
			return left is Integer
				? (Component)((Integer)left + right)
				: new SingleFactorComponent(new NumericFactor((Float)left + right));
		}

		public static Component operator -(Number left, IntegerFraction right)
		{
			return left is Integer
				? (Component)(((Integer)left) - right)
				: new SingleFactorComponent(new NumericFactor((Float)left - right));
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", IsNegative ? "⁻" : string.Empty, Math.Abs(Numerator), Denominator == 1 ? string.Empty : string.Format("/{0}", Denominator));
		}

		public override Expression Evaluate()
		{
			return Denominator == 1
				? Expressionizer.ToExpression(new NumericFactor(new Integer(Numerator)))
				: Expressionizer.ToExpression(this);
		}

		private Float ValueAsFloat()
		{
			return new Float((double)Numerator / Denominator);
		}

		private IntegerFraction Negative()
		{
			return new IntegerFraction(new Integer(-Numerator), new Integer(Denominator));
		}
	}
}
