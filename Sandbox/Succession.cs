using interval.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox;

class Succession
{
    static readonly int POINTS_NO = 300;

    static readonly int M1 = 1000;
    static readonly int N1 = 500;

    public Succession()
    {

    }

    public void Run()
    {
        List<double> points = LinSpace(0, 1, POINTS_NO);

        foreach (double point in points)
        {
            string? format = "N15";
            format = null;
            IntervalDouble dist = dist1(point);
            Console.WriteLine($"{point.ToString(format)} {dist}");
        }

    }

    List<double> LinSpace(double start, double end, int numPoints)
    {
        List<double> result = new();

        double step = (end - start) / (numPoints - 1);

        for (int i = 0; i < numPoints; ++i)
        {
            result.Add(start + step * i);
        }

        return result;
    }

    IntervalDouble dist1(double p) => dist(p, M1, N1);

    IntervalDouble dist(double p, int m, int n)
    {
        if (p <= 0 || p >= 1) return new IntervalDouble(0);

        IntervalDouble pI = new(p);
        IntervalDouble p1mp = new IntervalDouble(1) - pI;

        if (n > m)
        {
            int tempInt = m;
            m = n;
            n = tempInt;

            IntervalDouble tempI = pI;
            pI = p1mp;
            p1mp = tempI;
        }

        IntervalDouble factor = new(1);
        for (int n1 = 1; n1 <= n; n1++)
        {
            IntervalDouble num = new IntervalDouble(m + n1) * pI * p1mp;
            IntervalDouble denom = new(n1);
            factor *= num / denom;
        }

        factor *= new IntervalDouble(m + n + 1) * IntervalDouble.Pow(pI, m - n);

        return factor;
    }
}
