using System;

using Exercise;

namespace task8
{
    public class Program
    {
        public static void Main(String[] args)
        {
            BigInteger a = new BigInteger("18446744073709551616");
            BigInteger b = new BigInteger("36893488147419103232");
            BigInteger minus_a = new BigInteger("-18446744073709551616");

            Console.WriteLine("a + b = {0}", a + b);
            Console.WriteLine("a - b = {0}", a - b);
            Console.WriteLine("a * b = {0}", a * b);
            Console.WriteLine("a / b = {0}", a / b);
            Console.WriteLine("a % b = {0}", a % b);
            Console.WriteLine("-a = {0}", -a);
            Console.WriteLine("-b = {0}", -b);
            Console.WriteLine("-a == -a: {0}", -a == minus_a);
        }
    }
}

