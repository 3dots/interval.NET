using interval.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox;

class Point
{
    public double X;
    public IntervalDouble Y;

    public Point(double x, IntervalDouble y)
    {
        X = x;
        Y = y;
    }
}
