using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NativeBigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Exercise
{
    public class BigInteger : IEquatable<BigInteger>
    {
        protected List<int> _Digits = new List<int>();
        protected List<char> _Alphabet = new List<char>();
        protected Boolean _Negative = false;
        protected int _Base = 10;

        public List<int> Digits {
            get {
                return _Digits;
            }
        }

        public List<char> Alphabet {
            get {
                return _Alphabet;
            }
        }

        public Boolean Negative {
            get {
                return _Negative;
            }
        }

        public int Base {
            get {
                return _Base;
            }
        }

        public BigInteger()
        {
        }

        public BigInteger(String value)
        {
            if (value[0] == '-') {
                _Negative = true;
                value = value.Substring(1);
            }

            for (int i = value.Length - 1; i > -1; i--) {
                _Digits.Add(int.Parse(value[i].ToString()));
            }

            Trim();
        }

        public BigInteger(IEnumerable<int> digits, bool negative = false)
        {
            _Negative = negative;
            _Digits.AddRange(digits);
            _Digits.Reverse();
            Trim();
        }

        public BigInteger(IEnumerable<char> alphabet, NativeBigInteger value)
        {
            CheckAlphabet(alphabet);

            _Alphabet.Clear();
            _Alphabet.AddRange(alphabet);

            if (value < 0) {
                _Negative = true;
            }

            while (value > 0) {
                _Digits.Add((int) value % 10);
                value /= 10;
            }
        }

        public BigInteger(IEnumerable<char> alphabet, String value)
        {
            CheckAlphabet(alphabet);

            _Alphabet.Clear();
            _Alphabet.AddRange(alphabet);

            FromBase(alphabet.Count(), value.ToString());

            /*if (value[0] == '-') {
                _Negative = true;
                value = value.Substring(1);
            }

            // TODO: parse string from base X
            for (int i = value.Length - 1; i > -1; i--) {
                _Digits.Add(int.Parse(value[i].ToString()));
            }

            Trim();*/
        }

        public BigInteger(IEnumerable<char> alphabet, IEnumerable<char> value)
        {
            CheckAlphabet(alphabet);

            _Alphabet.Clear();
            _Alphabet.AddRange(alphabet);

            FromBase(alphabet.Count(), value.ToString());

            /*if (value.ElementAt(0) == '-') {
                _Negative = true;
                value = value.Skip(1);
            }

            // TODO: parse string from base X
            for (int i = value.Count() - 1; i > -1; i--) {
                _Digits.Add(int.Parse(value.ElementAt(i).ToString()));
            }

            Trim();*/
        }

        public BigInteger(IEnumerable<char> alphabet, StringBuilder value)
        {
            CheckAlphabet(alphabet);

            _Alphabet.Clear();
            _Alphabet.AddRange(alphabet);

            FromBase(alphabet.Count(), value.ToString());
        }

        public BigInteger(int numericBase, NativeBigInteger value)
        {
            SetAlphabet();

            if (numericBase > Alphabet.Count) {
                throw new ArgumentException("Too big numeric base");
            }

            // FromBase(numericBase, value.ToString());

            _Base = numericBase;

            if (value < 0)
                _Negative = true;

            while (value > 0) {
                _Digits.Add((int) value % 10);
                value /= 10;
            }
        }

        protected void FromBase(int numericBase, String value)
        {
            SetAlphabet();

            _Digits.Clear();
            _Base = numericBase;
            _Negative = false;

            if (value[0] == '-') {
                _Negative = true;
                value = value.Substring(1);
            }

            // _Digits.Add(1);
            BigInteger d = new BigInteger("1");

            for (int i = value.Count() - 1; i > -1; i--) {
                char ch = value[i];
                int digit = _Alphabet.IndexOf(ch);

                List<int> tmp_res = new List<int>();
                tmp_res.AddRange((this + (d * digit)).Digits);
                _Digits.Clear();
                _Digits.AddRange(tmp_res);

                d *= numericBase;
            }
        }

        protected void CheckAlphabet(IEnumerable<char> alphabet)
        {
            if (alphabet.Contains('-')) {
                throw new ArgumentException("Dash is not allowed within the alphabet");
            }

            if (!alphabet.Distinct().SequenceEqual(alphabet)) {
                throw new ArgumentException("Alphabet should not contain duplicates");
            }
        }

        protected IEnumerable<char> Range(char start, char end)
        {
            for (var c = start; c != Convert.ToChar((int) end + 1); c = Convert.ToChar((int) c + 1)) {
                yield return c;
            }
        }

        protected void SetAlphabet()
        {
            _Alphabet.AddRange(this.Range('0', '9'));
            _Alphabet.AddRange(this.Range('a', 'z'));
            _Alphabet.AddRange(this.Range('A', 'Z'));
        }

        protected BigInteger Trim()
        {
            for (int i = _Digits.Count - 1; i > -1; i--) {
                if (_Digits[i] != 0)
                    break;

                _Digits.RemoveAt(i);
            }

            return this;
        }

        public override int GetHashCode()
        {
            return NativeBigInteger.Parse(ToString()).GetHashCode();
        }

        public override String ToString()
        {
            String result = "";

            if (_Negative) {
                result += "-";
            }

            if (_Digits.Count < 1) {
                result += "0";

                return result;
            }

            for (int i = _Digits.Count - 1; i > -1; i--) {
                result += _Digits[i].ToString();
            }

            return result;
        }

        public static BigInteger operator -(BigInteger value)
        {
            BigInteger result = new BigInteger();

            result._Digits.AddRange(value._Digits);
            result._Negative = !value._Negative;

            return result.Trim();
        }

        public static BigInteger operator +(BigInteger a, BigInteger b)
        {
            if (a._Negative != b._Negative) {
                if (a._Negative) {
                    return a - b;
                } else {
                    return b - a;
                }
            }

            BigInteger result = new BigInteger();

            int mem = 0;

            List<int> v1 = new List<int>();
            List<int> v2 = new List<int>();
            List<int> v_res = new List<int>();

            v1.AddRange(a._Digits);
            v2.AddRange(b._Digits);

            int max_length = Math.Max(v1.Count, v2.Count);

            for (int i = 0; i < max_length; i++) {
                int tmp = mem;

                if (i < v1.Count)
                    tmp += v1[i];

                if (i < v2.Count)
                    tmp += v2[i];

                int digit = tmp % 10;

                mem = 0;

                if (tmp >= 10) {
                    mem = tmp / 10;
                }

                v_res.Add(digit);
            }

            while (mem > 0) {
                v_res.Add(mem % 10);
                mem /= 10;
            }

            result._Digits.AddRange(v_res);
            result._Negative = a._Negative;

            return result.Trim();
        }

        public static BigInteger operator -(BigInteger a, BigInteger b)
        {
            BigInteger result = new BigInteger();

            int mem = 0;

            List<int> v1 = new List<int>();
            List<int> v2 = new List<int>();
            List<int> v_res = new List<int>();

            if (a < b) {
                v1.AddRange(b._Digits);
                v2.AddRange(a._Digits);
                result._Negative = true;
            } else {
                v1.AddRange(a._Digits);
                v2.AddRange(b._Digits);
            }

            int max_length = Math.Max(v1.Count, v2.Count);

            for (int i = 0; i < max_length; i++) {
                int tmp = mem;

                mem = 0;

                if (i < v1.Count)
                    tmp += v1[i];

                if (i < v2.Count) {
                    if (tmp < v2[i]) {
                        mem = -1;
                        tmp += 10;
                    }

                    tmp -= v2[i];
                }

                v_res.Add(tmp);
            }

            if (mem < 0) {
                result._Negative = true;

                mem = Math.Abs(mem);

                while (mem > 0) {
                    v_res.Add(mem % 10);
                    mem /= 10;
                }
            }

            result._Digits.AddRange(v_res);

            return result.Trim();
        }

        public static BigInteger operator *(BigInteger a, BigInteger b)
        {
            BigInteger result = new BigInteger();

            int mem = 0;

            List<int> v1 = new List<int>();
            List<int> v2 = new List<int>();
            List<int> v_res = new List<int>();

            v1.AddRange(a._Digits);
            v2.AddRange(b._Digits);

            for (int t = 0; t < v2.Count; t++) {
                BigInteger tmp_result = new BigInteger();

                v_res.Clear();

                for (int j = 0; j < t; j++) {
                    v_res.Add(0);
                }

                for (int i = 0; i < v1.Count; i++) {
                    int tmp = 0;

                    if (i < v1.Count)
                        tmp += v1[i];

                    if (t < v2.Count)
                        tmp *= v2[t];

                    tmp += mem;

                    int digit = tmp % 10;

                    mem = 0;

                    if (tmp >= 10) {
                        mem = tmp / 10;
                    }

                    v_res.Add(digit);
                }

                while (mem > 0) {
                    v_res.Add(mem % 10);
                    mem /= 10;
                }

                tmp_result._Digits.AddRange(v_res);

                result += tmp_result;
            }

            if (a._Negative != b._Negative)
                result._Negative = true;

            return result;
        }

        public static BigInteger operator *(BigInteger a, int b)
        {
            BigInteger result = new BigInteger();

            int mem = 0;
            bool b_negative = (b < 0);

            for (int i = 0; i < a._Digits.Count; i++) {
                int tmp = mem + (a._Digits[i] * b);
                int digit = tmp % 10;

                mem = 0;

                if (tmp >= 10) {
                    mem = tmp / 10;
                }

                result._Digits.Add(digit);
            }

            while (mem > 0) {
                result._Digits.Add(mem % 10);
                mem /= 10;
            }

            if (a._Negative != b_negative)
                result._Negative = true;

            return result.Trim();
        }

        public static BigInteger operator /(BigInteger a, BigInteger b)
        {
            BigInteger result = new BigInteger();

            List<int> v1 = new List<int>();

            v1.AddRange(a._Digits);
            v1.Reverse();

            BigInteger d = new BigInteger(v1.Take(b._Digits.Count - 1));

            int next_digit = b._Digits.Count - 1;

            while (a > b) {
                if (next_digit > a._Digits.Count - 1)
                    break;

                d._Digits.Insert(0, v1[next_digit]);
                next_digit++;

                if (d < b && a._Digits.Count <= d._Digits.Count)
                    break;

                int res_digit = 9;

                while ((b * res_digit) > d && res_digit > -1) {
                    res_digit--;

                    if (res_digit < 0) {
                        if (next_digit == b._Digits.Count) {
                            break;
                        }

                        result._Digits.Add(res_digit);
                        d._Digits.Insert(0, v1[next_digit]);
                        next_digit++;
                        res_digit = 9;
                    }
                }

                if (res_digit > -1) {
                    result._Digits.Insert(0, res_digit);
                    d -= b * res_digit;
                }
            }

            return result.Trim();
        }

        public static BigInteger operator %(BigInteger a, BigInteger b)
        {
            BigInteger result = new BigInteger();

            result = a - ((a / b) * b);

            return result.Trim();
        }

        public static bool operator ==(BigInteger a, int _b)
        {
            if ((a as object) == null)
                return false;

            BigInteger b = new BigInteger(_b.ToString());
            return (a._Negative == b._Negative) && (Enumerable.SequenceEqual(a._Digits.OrderBy(t => t), b._Digits.OrderBy(t => t)));
        }

        public static bool operator !=(BigInteger a, int _b)
        {
            BigInteger b = new BigInteger(_b.ToString());
            return !(a == b);
        }

        public static bool operator ==(BigInteger a, BigInteger b)
        {
            if ((((a as object) == null) && ((b as object) != null)) || (((a as object) != null) && ((b as object) == null)))
                return false;

            if (((a as object) == null) && ((b as object) == null))
                return true;

            return (a._Negative == b._Negative) && (Enumerable.SequenceEqual(a._Digits.OrderBy(t => t), b._Digits.OrderBy(t => t)));
        }

        public static bool operator !=(BigInteger a, BigInteger b)
        {
            return !(a == b);
        }

        public static bool operator <(BigInteger a, BigInteger b)
        {
            if (a._Negative == b._Negative) {
                if (!a._Negative && !b._Negative) {
                    if (a._Digits.Count < b._Digits.Count)
                        return true;
                    else if (a._Digits.Count > b._Digits.Count)
                        return false;

                    int digits_count = a._Digits.Count;

                    for (int i = digits_count - 1; i > -1; i--) {
                        if (a._Digits[i] == b._Digits[i])
                            continue;

                        return (a._Digits[i] < b._Digits[i]);
                    }
                }

                // opposite if both would be positive
                if (a._Negative && b._Negative) {
                    if (a._Digits.Count < b._Digits.Count)
                        return false;
                    else if (a._Digits.Count > b._Digits.Count)
                        return true;

                    int digits_count = a._Digits.Count;

                    for (int i = digits_count - 1; i > -1; i--) {
                        if (a._Digits[i] == b._Digits[i])
                            continue;

                        return (a._Digits[i] > b._Digits[i]);
                    }
                }
            }

            if (a._Negative && !b._Negative)
                return true;

            return false;
        }

        public static bool operator >(BigInteger a, BigInteger b)
        {
            return (!(a < b) && !(a == b));
        }

        public bool Equals(BigInteger other)
        {
            if ((other as object) == null)
                return false;

            bool a = this.GetType() == other.GetType(),
            b = this._Negative == (other)._Negative,
            c = Enumerable.SequenceEqual(this._Digits.OrderBy(t => t), (other)._Digits.OrderBy(t => t));

            return ((a) && (b) && (c));
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            bool a = this.GetType() == other.GetType(),
            b = this._Negative == ((BigInteger) other)._Negative,
            c = Enumerable.SequenceEqual(this._Digits.OrderBy(t => t), ((BigInteger) other)._Digits.OrderBy(t => t));

            return ((a) && (b) && (c));
        }
    }
}
