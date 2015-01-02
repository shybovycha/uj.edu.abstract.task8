using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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

            Trim();
        }

        public BigInteger(IEnumerable<int> digits, bool negative = false)
        {
            Negative = negative;
            Digits.AddRange(digits);
            Digits.Reverse();
            Trim();
        }

        public override String ToString()
        {
            String result = "";

            if (Negative) {
                result += "-";
            }

            if (Digits.Count < 1) {
                result += "0";

                return result;
            }

            for (int i = Digits.Count - 1; i > -1; i--) {
                result += Digits[i].ToString();
            }

            return result;
        }

        protected BigInteger Trim()
        {
            for (int i = Digits.Count - 1; i > -1; i--) {
                if (Digits[i] != 0)
                    break;

                Digits.RemoveAt(i);
            }

            return this;
        }

        public static BigInteger operator -(BigInteger value)
        {
            BigInteger result = new BigInteger();

            result.Digits.AddRange(value.Digits);
            result.Negative = !value.Negative;

            return result.Trim();
        }

        public static BigInteger operator +(BigInteger a, BigInteger b)
        {
            if (a.Negative != b.Negative) {
                if (a.Negative) {
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

            v1.AddRange(a.Digits);
            v2.AddRange(b.Digits);

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

            result.Digits.AddRange(v_res);
            result.Negative = a.Negative;

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
                v1.AddRange(b.Digits);
                v2.AddRange(a.Digits);
                result.Negative = true;
            } else {
                v1.AddRange(a.Digits);
                v2.AddRange(b.Digits);
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
                result.Negative = true;

                mem = Math.Abs(mem);

                while (mem > 0) {
                    v_res.Add(mem % 10);
                    mem /= 10;
                }
            }

            result.Digits.AddRange(v_res);

            return result.Trim();
        }

        public static BigInteger operator *(BigInteger a, BigInteger b)
        {
            BigInteger result = new BigInteger();

            int mem = 0;

            List<int> v1 = new List<int>();
            List<int> v2 = new List<int>();
            List<int> v_res = new List<int>();

            v1.AddRange(a.Digits);
            v2.AddRange(b.Digits);

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

                tmp_result.Digits.AddRange(v_res);

                result += tmp_result;
            }

            if (a.Negative != b.Negative)
                result.Negative = true;

            return result;
        }

        public static BigInteger operator *(BigInteger a, int b)
        {
            BigInteger result = new BigInteger();

            int mem = 0;
            bool b_negative = (b < 0);

            for (int i = 0; i < a.Digits.Count; i++) {
                int tmp = mem + (a.Digits[i] * b);
                int digit = tmp % 10;

                mem = 0;

                if (tmp >= 10) {
                    mem = tmp / 10;
                }

                result.Digits.Add(digit);
            }

            while (mem > 0) {
                result.Digits.Add(mem % 10);
                mem /= 10;
            }

            if (a.Negative != b_negative)
                result.Negative = true;

            return result.Trim();
        }

        public static BigInteger operator /(BigInteger a, BigInteger b)
        {
            BigInteger result = new BigInteger();

            List<int> v1 = new List<int>();

            v1.AddRange(a.Digits);
            v1.Reverse();

            BigInteger d = new BigInteger(v1.Take(b.Digits.Count - 1));

            int next_digit = b.Digits.Count - 1;

            while (a > b) {
                if (next_digit > a.Digits.Count - 1)
                    break;

                d.Digits.Insert(0, v1[next_digit]);
                next_digit++;

                if (d < b && a.Digits.Count <= d.Digits.Count)
                    break;

                int res_digit = 9;

                while ((b * res_digit) > d && res_digit > -1) {
                    res_digit--;

                    if (res_digit < 0) {
                        if (next_digit == b.Digits.Count) {
                            break;
                        }

                        result.Digits.Add(res_digit);
                        d.Digits.Insert(0, v1[next_digit]);
                        next_digit++;
                        res_digit = 9;
                    }
                }

                if (res_digit > -1) {
                    result.Digits.Insert(0, res_digit);
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

        public static bool operator ==(BigInteger a, BigInteger b)
        {
            return (a.Negative == b.Negative) && (Enumerable.SequenceEqual(a.Digits.OrderBy(t => t), b.Digits.OrderBy(t => t)));
        }

        public static bool operator !=(BigInteger a, BigInteger b)
        {
            return !(a == b);
        }

        public static bool operator <(BigInteger a, BigInteger b)
        {
            if (a.Negative == b.Negative) {
                if (!a.Negative && !b.Negative) {
                    if (a.Digits.Count < b.Digits.Count)
                        return true;
                    else if (a.Digits.Count > b.Digits.Count)
                        return false;

                    int digits_count = a.Digits.Count;

                    for (int i = digits_count - 1; i > -1; i--) {
                        if (a.Digits[i] == b.Digits[i])
                            continue;

                        return (a.Digits[i] < b.Digits[i]);
                    }
                }

                // opposite if both would be positive
                if (a.Negative && b.Negative) {
                    if (a.Digits.Count < b.Digits.Count)
                        return false;
                    else if (a.Digits.Count > b.Digits.Count)
                        return true;

                    int digits_count = a.Digits.Count;

                    for (int i = digits_count - 1; i > -1; i--) {
                        if (a.Digits[i] == b.Digits[i])
                            continue;

                        return (a.Digits[i] > b.Digits[i]);
                    }
                }
            }

            if (a.Negative && !b.Negative)
                return true;

            return false;
        }

        public static bool operator >(BigInteger a, BigInteger b)
        {
            return (!(a < b) && !(a == b));
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
