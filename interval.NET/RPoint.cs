using interval.NET;
using IntervalDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interval.NET;

public class RPoint
{
    public double X;
    public IntervalDouble Y;

    public RPoint(double x, IntervalDouble y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}
