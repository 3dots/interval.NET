using interval.NET;
using System;
namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            //Entity e = new Entity("The Wallman", 20, 35);
            //e.Move(5, -10);
            //Console.WriteLine(e.XPosition + " " + e.YPosition);

            //Console.WriteLine(e.Add(1e-100, 1e100) == 1e100);

            //IntervalDoubleWrapper x = new(1e100);
            //IntervalDoubleWrapper y = new IntervalDoubleWrapper(1e-100);

            //IntervalDoubleWrapper sum = IntervalDoubleWrapper.Add(x, y);

            //Console.WriteLine(sum.Upper);
            //Console.WriteLine(sum.Lower);

            //sum = IntervalDoubleWrapper.Add(sum, y);

            //Console.WriteLine(sum.Upper);
            //Console.WriteLine(sum.Lower);

            IntervalDouble x = new(1e100);
            IntervalDouble y = new(1e-100);

            IntervalDouble sum = x + y;
            Console.WriteLine(sum.Upper);
            Console.WriteLine(sum.Lower);

            sum = sum + y;

            Console.WriteLine(sum.Upper);
            Console.WriteLine(sum.Lower);

            Console.ReadKey();
        }
    }
}