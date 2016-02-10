using System;
using Titanium.Core.Exceptions;
using Titanium.Core.Expressions;
using Titanium.Core.Factors;
using Titanium.Core.Numbers;
using Titanium.Core.Reducer;

namespace Titanium.Core.Components
{
	public class IntegerFraction : Component
	{
		internal bool IsNegative { get; private set; }
		internal int Numerator { get; private set; }
		internal int Denominator { get; private set; }

		internal IntegerFraction(int value)
		{
			Ctor(value, 1);
		}

		internal IntegerFraction(Integer value)
		{
			Ctor(value.Value, 1);
		}

		internal IntegerFraction(int numerator, int denominator)
		{
			Ctor(numerator, denominator);
		}

		internal IntegerFraction(Integer numerator, int denominator)
		{
			Ctor(numerator.Value, denominator);
		}

		internal IntegerFraction(int numerator, Integer denominator)
		{
			Ctor(numerator, denominator.Value);
		}

		internal IntegerFraction(Integer numerator, Integer denominator)
		{
			Ctor(numerator.Value, denominator.Value);
		}

		private void Ctor(int numerator, int denominator)
		{
			if (denominator == 0)
			{
				throw new DivideByZeroException();
			}

			var gcd = Integer.GreatestCommonDivisor(numerator, denominator);

			IsNegative = Math.Sign(numerator) * Math.Sign(denominator) < 0;

			Numerator = Math.Abs(numerator / gcd) * (IsNegative ? -1 : 1);
			Denominator = Math.Abs(denominator / gcd);
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

		public static Component operator ^(IntegerFraction left, IntegerFraction right)
		{
			if (left.Numerator == 0) return new IntegerFraction(0);
			return new DualFactorComponent(Factorizer.ToFactor(left), Factorizer.ToFactor(right), ComponentType.Exponent);
		}

		public static IntegerFraction operator +(IntegerFraction left, Integer right)
		{
			return left + new IntegerFraction(right, 1);
		}

		public static IntegerFraction operator -(IntegerFraction left, Integer right)
		{
			return left - new IntegerFraction(right, 1);
		}

		public static IntegerFraction operator *(IntegerFraction left, Integer right)
		{
			return left * new IntegerFraction(right, 1);
		}

		public static IntegerFraction operator /(IntegerFraction left, Integer right)
		{
			return left / new IntegerFraction(right, 1);
		}

		public static Component operator ^(IntegerFraction left, Integer right)
		{
			throw new NotImplementedException();
		}

		public static IntegerFraction operator +(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, 1) + right;
		}

		public static IntegerFraction operator -(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, 1) - right;
		}

		public static IntegerFraction operator *(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, 1) * right;
		}

		public static IntegerFraction operator /(Integer left, IntegerFraction right)
		{
			return new IntegerFraction(left, 1) / right;
		}

		public static Component operator ^(Integer left, IntegerFraction right)
		{
			if (right.IsNegative)
			{
				throw new NonRealResultException();
			}

			var rawResult = Math.Pow(left.Value, right.ValueAsFloat().Value);

			if (Float.IsWholeNumber(rawResult))
			{
				return new IntegerFraction((int)rawResult);
			}

			return new DualFactorComponent(new NumericFactor(left), Factorizer.ToFactor(right), ComponentType.Exponent);
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
			if (right.IsNegative)
			{
				throw new NonRealResultException();
			}

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

		public static Number operator ^(Float left, IntegerFraction right)
		{
			return left ^ right.ValueAsFloat();
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
