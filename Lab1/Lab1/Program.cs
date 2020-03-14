using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            // variant 1
            while (true) // might cause exceptions
            {
                try
                {
                    Task1();
                    Task2();
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("False argument format. Please don't use any string");
                    continue;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("False argument format. Enter 2 numbers");
                    continue;
                }
            }
            Task3(2, 3, 0.00000001); // a, b, epsilon
            Console.ReadLine();
        }
        static void Task1() // sum of 2 integer numbers of any size
        {
            Console.WriteLine("Task 1: \nEnter 2 numbers");
            var s = Console.ReadLine();
            string[] pars = s.Split();
            if (pars.Length < 2) throw new ArgumentException("Too less parameters");
            long a = Convert.ToInt64(pars[0]), b = Convert.ToInt64(pars[1]);
            Console.WriteLine("Their sum is " + Sum(a, b));
        }
        static void Task2()
        {
            Console.WriteLine("Task 2: \nEnter k then x");
            var s = Console.ReadLine();
            var pars = s.Split();
            if (pars.Length < 2) throw new ArgumentException("Too less parameters");
            int c = Convert.ToInt32(pars[0]);
            double d = Convert.ToDouble(pars[1]);
            Console.WriteLine($"Sum equals to {Sum2(c, d)}");
        }
        static void Task3(double a, double b, double eps)
        {
            double x;
            x = (F0(a) * F2(a) > 0) ? a : b;
            while (Math.Abs(F0(x)) > eps)
            {
                x -= (F0(x) / F1(x));
            }
            Console.WriteLine($"Task 3: \nNull of function on [{a},{b}] is " + x);
        }
        static double F0(double x)
        {
            return 3 * Math.Sin(Math.Sqrt(x)) + 0.35 * x - 3.8;
        }
        static double F1(double x)
        {
            return 1.5 * Math.Cos(Math.Sqrt(x)) / Math.Sqrt(x) + 0.35;
        }
        static double F2(double x)
        {
            return 1.5 * ((-(Math.Sin(Math.Sqrt(x)) / 2) - (Math.Cos(Math.Sqrt(x)) / (2 * Math.Sqrt(x)))) / x);
        }
        static long Sum(long a, long b) // simple overflow check
        {
            if (((Math.Abs(a) < Math.Abs(long.MaxValue / 2)) && (Math.Abs(b) < Math.Abs(long.MaxValue/2))) || ((a * b) < 0)) return a + b;
            // simple overflow check. 
            else throw new ArgumentException("Overflow");
        }
        static double Sum2(int k, double x)
        {
            double sum = 0;
            int fact = 1;
            for (int i = 0; i < k; i++)
            {
                fact *= (i + 1);
                sum += (Math.Pow(Math.Log(3), i + 1) * Math.Pow(x, i + 1))/fact;
            }
            return sum;
        }
    }
}
