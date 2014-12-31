using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise
{
	public class BigInteger
	{
		public List<int> Digits = new List<int>();
		public Boolean Negative = false;

		public BigInteger()
		{
		}

		public BigInteger(String value)
		{
			if (value[0] == '-') {
				Negative = true;
				value = value.Substring(1);
			}
				
			for (int i = value.Length - 1; i > -1; i--) {
				Digits.Add(int.Parse(value[i].ToString()));
			}
		}

		public override String ToString()
		{
			String result = "";

			if (Negative) {
				result += "-";
			}

			for (int i = Digits.Count - 1; i > -1; i--) {
				result += Digits[i].ToString();
			}

			return result;
		}

		public static BigInteger operator -(BigInteger value)
		{
			BigInteger result = new BigInteger();

			result.Digits.AddRange(value.Digits);
			result.Negative = !value.Negative;

			return result;
		}

		public static BigInteger operator +(BigInteger a, BigInteger b)
		{
			BigInteger result = new BigInteger();

			return result;
		}

		public static BigInteger operator -(BigInteger a, BigInteger b)
		{
			BigInteger result = new BigInteger();

			return result;
		}

		public static BigInteger operator *(BigInteger a, BigInteger b)
		{
			BigInteger result = new BigInteger();

			return result;
		}

		public static BigInteger operator /(BigInteger a, BigInteger b)
		{
			BigInteger result = new BigInteger();

			return result;
		}

		public static BigInteger operator %(BigInteger a, BigInteger b)
		{
			BigInteger result = new BigInteger();

			result = a - ((a / b) * b);

			return result;
		}

		public static bool operator ==(BigInteger a, BigInteger b)
		{
			return (a.Negative == b.Negative) && (Enumerable.SequenceEqual(a.Digits.OrderBy(t => t), b.Digits.OrderBy(t => t)));
		}

		public static bool operator !=(BigInteger a, BigInteger b)
		{
			return !(a == b);
		}

		public override bool Equals(object other)
		{
			bool a = this.GetType() == other.GetType(),
			b = this.Negative == ((BigInteger) other).Negative,
			c = Enumerable.SequenceEqual(this.Digits.OrderBy(t => t), ((BigInteger) other).Digits.OrderBy(t => t));

			return ((a) && (b) && (c));
		}
	}
}
