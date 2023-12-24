using System;
using IntervalDotNET;
namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Entity e = new Entity("The Wallman", 20, 35);
            e.Move(5, -10);
            Console.WriteLine(e.XPosition + " " + e.YPosition);

            Console.WriteLine(e.Add(1e-100, 1e100) == 1e100);

            Console.ReadKey();
        }
    }
}