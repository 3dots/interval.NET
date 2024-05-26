using interval.NET;
using System;
namespace Sandbox;

class Program
{
    static void Main(string[] args)
    {
        //IntervalDouble x = new IntervalDouble(double.NaN);

        //Console.WriteLine(x);

        new Succession().Run();
        
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}