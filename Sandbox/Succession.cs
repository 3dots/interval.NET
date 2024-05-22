using interval.NET;
using Python.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Sandbox;

class Succession
{
    #region Fields

    static readonly int POINTS_NO_INITIAL = 400;
    static readonly double POS_THRESHOLD = 0.1;
    static readonly int POINTS_NO_FOCUS = 400;

    static readonly int M1 = 1000;
    static readonly int N1 = 500;

    static readonly int M2 = 800;
    static readonly int N2 = 430;

    #endregion

    #region Events

    public Succession()
    {

    }

    public void Run()
    {
        //double diff = -0.02;
        //Console.WriteLine($"{diff} {distDiff(diff)}");
        //double diff = 0.02;
        //Console.WriteLine($"{diff} {distDiff(diff)}");

        //Console.WriteLine($"{distDiffIntegrand(0.65, -0.02)}");

        //List<RPoint> diffDist = new();
        List<RPoint> diffDist = ComputeDist(nameof(diffDist), distDiff, -1, 1);
        ShowGraphs(new List<List<RPoint>>
        {
            ComputeDist(nameof(dist1), dist1),
            ComputeDist(nameof(dist2), dist2)
        }, diffDist);
    }

    List<RPoint> ComputeDist(string name, Func<double, IntervalDouble> dist, double start = 0, double end = 1)
    {
        double[] points = LinSpace(start, end, POINTS_NO_INITIAL);
        List<RPoint> distList = Compute(name, points, dist);

        int indexFirstRealValue = distList.FindIndex(p => p.Y.Lower >= POS_THRESHOLD);
        int indexLastRealValue = distList.FindIndex(indexFirstRealValue, p => p.Y.Lower < POS_THRESHOLD);

        points = LinSpace(distList[indexFirstRealValue].X, distList[indexLastRealValue].X, POINTS_NO_FOCUS);
        distList.AddRange(Compute(name, points, dist));
        return distList;
    }

    void ShowGraphs(List<List<RPoint>> dists, List<RPoint> diff)
    {
        Runtime.PythonDLL = "C:\\Users\\3dot\\AppData\\Local\\Programs\\Python\\Python312\\python312.dll";
        PythonEngine.Initialize();
        using (Py.GIL())
        using (PyModule scope = Py.CreateScope())
        {
            scope.Import("matplotlib.pyplot", "plt");

            scope.Exec("plt.figure(figsize=(6, 8))");
            scope.Exec("plt.subplot(2, 1, 1)");
            scope.Exec("plt.title('p1, p2')");          

            foreach (List<RPoint> distList in dists)
            {
                Plot(scope, distList);
            }

            scope.Exec("plt.subplot(2, 1, 2)");
            scope.Exec("plt.title('p1 - p2')");
            Plot(scope, diff);

            scope.Exec("plt.tight_layout()");
            scope.Exec("plt.show()");
        }
    }

    void Plot(PyModule scope, List<RPoint> distList)
    {
        int count = distList.Count;
        double[] pointsArr = new double[count];
        double[] dist_lower = new double[count];
        double[] dist_upper = new double[count];

        for (int i = 0; i < distList.Count; i++)
        {
            RPoint dist = distList[i];
            pointsArr[i] = dist.X;
            dist_lower[i] = dist.Y.Lower;
            dist_upper[i] = dist.Y.Upper;
        }

        PyObject pointsPy = pointsArr.ToPython();
        PyObject dist_lowerPy = dist_lower.ToPython();
        PyObject dist_upperPy = dist_upper.ToPython();

        scope.Set(nameof(pointsPy), pointsPy);
        scope.Set(nameof(dist_lowerPy), dist_lowerPy);
        scope.Set(nameof(dist_upperPy), dist_upperPy);
        scope.Exec($"plt.scatter({nameof(pointsPy)}, {nameof(dist_lowerPy)}, s=1)");
        scope.Exec($"plt.scatter({nameof(pointsPy)}, {nameof(dist_upperPy)}, s=1)");
    }

    #endregion

    #region Helpers

    double[] LinSpace(double start, double end, int numPoints)
    {
        double[] result = new double[numPoints];

        double step = (end - start) / (numPoints - 1);

        for (int i = 0; i < numPoints; ++i)
        {
            result[i] = start + step * i;
        }

        return result;
    }

    List<RPoint> Compute(string name, double[] points, Func<double, IntervalDouble> dist)
    {
        Console.WriteLine(name);
        ConcurrentBag<RPoint> bag = new();
        Parallel.ForEach(points, point => {
            IntervalDouble res = dist(point);
            //Console.WriteLine($"{name} {point} {res.Lower} {res.Upper}");
            bag.Add(new RPoint(point, res));
        });

        return bag.ToList();
    }    

    #endregion

    #region Distributions

    IntervalDouble dist1(double p) => dist(p, M1, N1);

    IntervalDouble dist2(double p) => dist(p, M2, N2);

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

        //Console.WriteLine($"dist\t{factor.Lower}\t{factor.Upper}\t{p}");
        return factor;
    }

    IntervalDouble distDiff(double diff)
    {
        double errTolerance = 1e-12;
        List<RPoint> integralEvolution = Odeint.IntegrateAdaptive((IntervalDouble x, double t) => distDiffIntegrand(t, diff),
            new IntervalDouble(0), 0, 1 - diff, 0.01, errTolerance, errTolerance);
        //foreach (RPoint p in integralEvolution)
        //{
        //    Console.WriteLine(p.ToString());
        //}
        return integralEvolution.Last().Y;
    }

    IntervalDouble distDiffIntegrand(double p, double diff) => dist1(p) * dist2(p - diff);

    #endregion
}
