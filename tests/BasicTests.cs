using NUnit.Framework;
using System;

﻿using Exercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using NativeBigInteger = System.Numerics.BigInteger;

namespace Tests
{
	[TestFixture ()]
	public class BasicTests
	{
		[Test ()]
		public void Constructors()
		{
			var nbi = NativeBigInteger.Parse(this.OneTwoThree);
			new BigInteger(this.Alphabet, nbi);
			new BigInteger(this.Alphabet, this.OneTwoThree);
			new BigInteger(this.Alphabet, this.OneTwoThree.ToArray());
			new BigInteger(this.Alphabet, new StringBuilder(this.OneTwoThree));
			new BigInteger(this.Alphabet.Length, nbi);
		}

		[Test ()]
		public void ToStringImplementation()
		{
			var x = new BigInteger(this.Alphabet, this.OneTwoThree);
			Assert.AreEqual(this.OneTwoThree, x.ToString());
			var r = new Random();
			var a = this.Alphabet;
			var max = a.Count();
			var length = 100;
			var number = new char[length];
			for (int i = 0; i < length; i++)
			{
				var digit = a[r.Next(max)];
				number[i] = digit;
			}
			var y = new BigInteger(this.Alphabet, number);
			Assert.AreEqual(new String(number), y.ToString());
		}

		[Test ()]
		public void ExceedLongWhileMultiplying()
		{
			int power = 5;
			var expected = NativeBigInteger.Pow(Int64.MaxValue, power);
			var multiplied = new BigInteger(10, NativeBigInteger.One);
			var element = new BigInteger(10, new NativeBigInteger(Int64.MaxValue));
			for (int i = 0; i < power; i++)
			{
				multiplied *= element;
			}
			Assert.AreEqual(expected.ToString(), multiplied.ToString());
		}

		[Test ()]
		public void StringFormatting()
		{
			var formatBase10 = "{0:b10}";
			var x = new BigInteger(this.Alphabet.Take(16), this.OneToNineBase16);
			Assert.AreEqual(this.OneToNineBase10, String.Format(formatBase10, x));
			var formatBase17 = "{0:b17}";
			Assert.AreEqual(this.OneToNineBase17, String.Format(formatBase17, x));
		}

		[Test ()]
		public void Comparers()
		{
			var x = new BigInteger(this.Alphabet.Take(16), this.OneToNineBase16);
			var y = new BigInteger(this.Alphabet.Take(17), this.OneToNineBase17);
			Assert.IsTrue((x as IEquatable<BigInteger>).Equals(y));
			Assert.IsTrue(x == y);
		}

		[Test ()]
		public void GetHashCodeAndEqualsObject()
		{
			var x = new BigInteger(this.Alphabet.Take(10), this.OneToNineBase10);
			Assert.AreEqual(NativeBigInteger.Parse(this.OneToNineBase10).GetHashCode(), x.GetHashCode());
			Assert.AreNotEqual(true, x.Equals((object)null));
		}

		[Test ()]
		public void AlphabetWithoutNegativeSign()
		{
			var random = new Random();
			var alphabet = this.Alphabet.ToList();
			this.TestFailingAlphabet(() => this.WithInserted(alphabet, random.Next(alphabet.Count()), '-'));
		}

		[Test ()]
		public void AlphabetWithoutDuplications()
		{
			var random = new Random();
			for (int j = 0; j < 10; j++)
			{
				this.TestFailingAlphabet(() =>
					{
						var alphabet = this.Alphabet.ToList();
						for (int i = 0; i <= j; i++)
						{
							char what = this.Alphabet[random.Next(this.Alphabet.Count())];
							int where = random.Next(alphabet.Count());
							alphabet = this.WithInserted(alphabet, where, what);
						}
						return alphabet;
					});
			}
		}

		[Test ()]
		public void OperateUponVariousAlphabets()
		{
			var a1 = this.Alphabet;
			var r = new Random();
			for (int systemBase = 2; systemBase <= a1.Count(); systemBase++)
			{
				var n1 = this.RandomNumber(a1, r.Next(1, 10), r);
				var a2 = this.RandomizedAlphabet(systemBase, r);
				var n2 = this.RandomNumber(a2, r.Next(1, 10), r);
				var result = n1 + n2;
				result = n1 - n2;
				result = n1 * n2;
				if (n2 != new BigInteger(a2, a2[0].ToString()))
				{
					result = n1 / n2;
				}
			}
		}

		[Test ()]
		public void Operators()
		{
			var x = new BigInteger(this.Alphabet.Take(16), this.OneToNineBase16);
			var y = -(new BigInteger(this.Alphabet.Take(17), this.OneToNineBase17));
			var doubledPositive = new BigInteger(10, NativeBigInteger.Parse(this.OneToNineBase10) * 2);
			var multipliedPositive = new BigInteger(10, NativeBigInteger.Pow(NativeBigInteger.Parse(this.OneToNineBase10), 2));
			Assert.IsTrue(x + y == 0);
			Assert.IsTrue(x - y == doubledPositive);
			Assert.IsTrue(x * y == -multipliedPositive);
			Assert.IsTrue(doubledPositive / y == -2);
		}

		[Test ()]
		public void DefaultAlphabet()
		{
			var x = new BigInteger(this.Alphabet.Count(), NativeBigInteger.Zero);
			Assert.IsTrue(this.Alphabet.SequenceEqual(x.Alphabet));
		}

		[Test ()]
		public void ProcessNumbersInDifferentSystemBase()
		{
			var x = new BigInteger(this.Alphabet.Take(16), this.OneToNineBase16);
			var y = new BigInteger(this.Alphabet.Take(17), this.OneToNineBase17);
			Func<BigInteger, BigInteger, BigInteger>[] actions = new Func<BigInteger, BigInteger, BigInteger>[]
			{
				(a, b) => a + b,
				(a, b) => b + a,
				(a, b) => a * b,
				(a, b) => b * a,
				(a, b) => a / b,
				(a, b) => b / a,
				(a, b) => a % b,
				(a, b) => b % a
			};
			foreach (var action in actions)
			{
				Assert.AreEqual(17, action(x, y).Base);
			}
		}

		[Test ()]
		public void ParameterNamesStartingWithSmallLetter()
		{
			var types = typeof(BigInteger).Assembly.GetTypes();
			var memberFilter = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
			Func<ParameterInfo, bool> parameterFilter = p => Char.IsUpper(p.Name.First());
			Func<IEnumerable<ParameterInfo>, MemberInfo, Type, string> message = (parameters, member, type) =>
			{
				var arrified = parameters.Select(pi => pi.ToString()).ToArray();
				string result = String.Format("Invalid parameters \"{0}\" in member \"{1}\" of type \"{2}\".", String.Join(", ", arrified), member, type);
				return result;
			};
			foreach (var type in types)
			{
				var settersIndexersAndOtherMethods = type.GetMethods(memberFilter);
				foreach (var method in settersIndexersAndOtherMethods)
				{
					var parameters = method.GetParameters()
						.Where(parameterFilter);
					Assert.AreEqual(0, parameters.Count(), message(parameters, method, type));
				}
				var constructors = type.GetConstructors(memberFilter | BindingFlags.CreateInstance);
				foreach (var constructor in constructors)
				{
					var parameters = constructor.GetParameters()
						.Where(parameterFilter);
					Assert.AreEqual(0, parameters.Count(), message(parameters, constructor, type));
				}
			}
		}

		[Test ()]
		public void MethodNamesStartingWithBigLetter()
		{
			var types = typeof(BigInteger).Assembly.GetTypes();
			var memberFilter = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
			Func<MethodInfo, bool> methodFilter = mi =>
				((mi.Attributes & MethodAttributes.SpecialName) != MethodAttributes.SpecialName) &&
				(Char.IsLower(mi.Name.First(c => c != '_')));
			foreach (var type in types)
			{
				var oddOnes = type.GetMethods(memberFilter).Where(methodFilter);
				if (oddOnes.Any())
				{
					var methodNames = oddOnes.Select(oddOne => oddOne.ToString()).ToArray();
					var message = String.Format("Invalid method name(s) \"{0}\" of type \"{1}\".", String.Join(", ", methodNames), type);
					Assert.Fail(message);
				}
			}
		}

		[Test ()]
		public void NoPublicFieldsAndPropertySetters()
		{
			var types = typeof(BigInteger).Assembly.GetTypes();
			var withPublicSetter = types.Where(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Count(p => p.GetSetMethod(false) != null) > 0)
				.Select(t => t.FullName).ToArray();
			var withPublicSetterMessage = String.Format("Detected type(s) {0} with public setter, which is not allowed.", String.Join(", ", withPublicSetter));
			Assert.AreEqual(0, withPublicSetter.Count(), withPublicSetterMessage);
			var withPublicField = types.Where(t => !t.IsEnum)
				.Where(t => t.GetFields(BindingFlags.Public | BindingFlags.Instance).Count() > 0)
				.Where(t => !t.Name.StartsWith("<>c__DisplayClass")) // generated by LINQ ForEach and Any methods
				.Where(t => !Regex.IsMatch(t.Name, "[<]([a-z]|[A-Z])*[>]d__.")) // generated by yield return construct (example "<GetEnumerator>d__0" or "<Range>d__0", depends on method name)
				.ToDictionary(t => t.FullName, t => t.GetFields(BindingFlags.Public | BindingFlags.Instance));
			if (withPublicField.Any())
			{
				var oddOne = withPublicField.First();
				var message = String.Format("Detected type \"{0}\" with public filed(s) \"{1}\", which is not allowed.",
					oddOne.Key,
					String.Join(", ", oddOne.Value.Select(fi => fi.ToString())));
				Assert.Fail(message);
			}
		}

		private MethodInfo GetMethodInfo<T>(Expression<Action<T>> e)
		{
			var mce = e.Body as MethodCallExpression;
			return mce.Method;
		}

		private List<T> WithInserted<T>(List<T> l, int index, T item)
		{
			l.Insert(index, item);
			return l;
		}

		private void TestFailingAlphabet(Func<IEnumerable<char>> alphabet)
		{
			var nbi = NativeBigInteger.Parse(this.OneTwoThree);
			Func<BigInteger>[] forbidden = new Func<BigInteger>[]
			{
				() => new BigInteger(alphabet(), nbi),
				() => new BigInteger(alphabet(), this.OneTwoThree),
				() => new BigInteger(alphabet(), this.OneTwoThree.ToArray()),
				() => new BigInteger(alphabet(), new StringBuilder(this.OneTwoThree)),
				() => new BigInteger(alphabet().Count(), nbi)
			};
			foreach (var action in forbidden)
			{
				try
				{
					var x = action();
					throw new Exception("Invalid action performed.");
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private IEnumerable<char> Range(char start, char end)
		{
			for (var c = start; c != Convert.ToChar((int)end + 1); c = Convert.ToChar((int)c + 1))
			{
				yield return c;
			}
		}

		private string OneToNineBase10
		{
			get
			{
				return String.Join(String.Empty, Enumerable.Range(1, 9).Select(i => i.ToString()).ToArray());
			}
		}

		private string OneToNineBase16
		{
			get
			{
				return "75bcd15";
			}
		}

		private string OneToNineBase17
		{
			get
			{
				return "51g2a21";
			}
		}

		private string OneTwoThree
		{
			get
			{
				return "123";
			}
		}

		private BigInteger RandomNumber(char[] alphabet, int digits, Random r)
		{
			StringBuilder value = new StringBuilder();
			for (int i = 0; i < digits; i++)
			{
				var digit = alphabet[r.Next(alphabet.Count())];
				value.Append(digit);
			}
			return new BigInteger(alphabet, value);
		}

		private char[] RandomizedAlphabet(int systemBase, Random r)
		{
			var alphabet = new List<char>();
			while (alphabet.Count() < systemBase)
			{
				int x = r.Next(Char.MaxValue - Char.MinValue);
				var c = Convert.ToChar(x);
				if ((!alphabet.Contains(c)) && (c != '-'))
				{
					alphabet.Add(c);
				}
			}
			return alphabet.ToArray();
		}

		private char[] Alphabet
		{
			get
			{
				var alphabet = new List<char>();
				alphabet.AddRange(this.Range('0', '9'));
				alphabet.AddRange(this.Range('a', 'z'));
				alphabet.AddRange(this.Range('A', 'Z'));
				return alphabet.ToArray();
			}
		}
	}
}