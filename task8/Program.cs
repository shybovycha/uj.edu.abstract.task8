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

            Console.WriteLine("a = {0}", a);
            Console.WriteLine("b = {0}", b);

            Console.WriteLine("-a = {0}", -a);
            Console.WriteLine("-b = {0}", -b);
            Console.WriteLine("-a == `-a`: {0}", -a == minus_a);

            Console.WriteLine("a < b {0}", a < b);
            Console.WriteLine("b < a = {0}", b < a);
            Console.WriteLine("-a < a: {0}", -a < a);

            Console.WriteLine("a + b = {0}", a + b);
            Console.WriteLine("-a - b = {0}", -a - b);
            Console.WriteLine("a - b = {0}", a - b);
            Console.WriteLine("a * b = {0}", a * b);
            Console.WriteLine("b / a = {0}", b / a);
            Console.WriteLine("a % b = {0}", a % b);

            Console.WriteLine("5157536727 / 234276 = {0}", new BigInteger("5157536727") / new BigInteger("234276"));
        }
    }
}

