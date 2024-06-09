using IntervalDotNET;
using System;
namespace Sandbox;

class Program
{
    static void Main(string[] args)
    {
        //var x = new IntervalDouble(1.0);
        //var y = new IntervalDouble(1e-35);

        //IntervalDouble sum = x + y;

        //Console.WriteLine(sum);
        //Console.WriteLine(sum.Lower == sum.Upper);

        new Succession().Run();
        
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}