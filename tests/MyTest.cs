using System;

using NUnit.Framework;
using Exercise;

namespace tests
{
    [TestFixture()]
    public class MyTest
    {
        [Test()]
        public void Constructors()
        {
            BigInteger a = new BigInteger();
            Assert.AreEqual(a.Digits.Count, 0);
            Assert.IsFalse(a.Negative);

            BigInteger b = new BigInteger("-1234567890987654321");
            Assert.AreEqual(b.Digits.Count, 19);
            Assert.IsTrue(b.Negative);
        }

        [Test()]
        public void ToStringImplementation()
        {
            BigInteger a = new BigInteger();
            Assert.AreEqual(1, a.ToString().Length);

            BigInteger b = new BigInteger("-1234567890987654321");
            Assert.AreEqual("-1234567890987654321", b.ToString());
        }

        [Test()]
        public void Operators()
        {
            // a = 1 << 64
            BigInteger a = new BigInteger("18446744073709551616");

            // b = 1 << 65
            BigInteger b = new BigInteger("36893488147419103232");

            // c = a - 1
            BigInteger c = new BigInteger("18446744073709551615");

            // d = 3
            BigInteger d = new BigInteger("3");

            BigInteger zero = new BigInteger("0");
            BigInteger one = new BigInteger("1");
            BigInteger two = new BigInteger("2");
            BigInteger ten = new BigInteger("10");

            // a + b == 55340232221128654848
            BigInteger a_plus_b = new BigInteger("55340232221128654848");

            // a - b == -a
            BigInteger minus_a = new BigInteger("-18446744073709551616");

            // b - a == a
            // BigInteger b_minus_a = new BigInteger("18446744073709551616");

            // a * b == 680564733841876926926749214863536422912
            BigInteger a_mul_b = new BigInteger("680564733841876926926749214863536422912");

            // b / a == 2
            // BigInteger b_div_a = new BigInteger("2");

            // b / c == 2
            // BigInteger b_div_c = new BigInteger("2");

            // b % a == 0
            // BigInteger b_mod_a = new BigInteger("0");

            // a % c == 1
            // BigInteger a_mod_c = new BigInteger("1");

            // b / d == 12297829382473034410
            BigInteger b_div_d = new BigInteger("12297829382473034410");

            // b % d == 2
            // BigInteger b_mod_d = new BigInteger("2");

            Assert.AreEqual("123", new BigInteger("0123456789abcdef", "7b").ToDecimalString());
            Assert.AreEqual("7b", new BigInteger(16, System.Numerics.BigInteger.Parse("123")).ToBase());
            Assert.AreEqual(-a, minus_a);
            Assert.AreEqual(a - b, minus_a);
            Assert.AreEqual(a + b, a_plus_b);
            Assert.AreEqual(a * b, a_mul_b);
            Assert.AreEqual(a / b, zero);
            Assert.AreEqual(a % b, a);
            Assert.AreEqual(b + a, a_plus_b);
            Assert.AreEqual(b - a, a);
            Assert.AreEqual(b * a, a_mul_b);
            Assert.AreEqual(b / a, two);
            Assert.AreEqual(b % a, zero);
        }
    }
}

