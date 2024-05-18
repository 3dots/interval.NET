using interval.NET;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox;

class Succession
{
    #region Fields

    static readonly int POINTS_NO_INITIAL = 1000;
    static readonly double POS_THRESHOLD = 0.001;
    static readonly int POINTS_NO_FOCUS = 1000;

    static readonly int M1 = 1000;
    static readonly int N1 = 500;

    static readonly int M2 = 1000;
    static readonly int N2 = 300;

    #endregion

    #region Events

    public Succession()
    {

    }

    public void Run()
    {
        double[] points = LinSpace(0, 1, POINTS_NO_INITIAL);
        List<Point> distList = new();
        Compute(distList, points, dist1);

        int indexFirstRealValue = distList.FindIndex(p => p.Y.Lower >= POS_THRESHOLD);
        int indexLastRealValue = distList.FindIndex(indexFirstRealValue, p => p.Y.Lower < POS_THRESHOLD);

        points = LinSpace(distList[indexFirstRealValue].X, distList[indexLastRealValue].X, POINTS_NO_FOCUS);
        Compute(distList, points, dist1);

        Runtime.PythonDLL = "C:\\Users\\3dot\\AppData\\Local\\Programs\\Python\\Python312\\python312.dll";
        PythonEngine.Initialize();
        using (Py.GIL())
        using (PyModule scope = Py.CreateScope())
        {
            scope.Import("matplotlib.pyplot", "plt");

            int count = distList.Count;
            double[] pointsArr = new double[count];
            double[] dist_lower = new double[count];
            double[] dist_upper = new double[count];

            for (int i = 0; i < distList.Count; i++)
            {
                Point dist = distList[i];
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

            scope.Exec($"plt.figure(figsize=(6, 8))");
            scope.Exec($"plt.scatter({nameof(pointsPy)}, {nameof(dist_lowerPy)}, s=1)");
            scope.Exec($"plt.scatter({nameof(pointsPy)}, {nameof(dist_upperPy)}, s=1)");
            scope.Exec("plt.tight_layout()");
            scope.Exec("plt.show()");
        }
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

    void Compute(List<Point> list, double[] points, Func<double, IntervalDouble> dist)
    {
        foreach (double point in points)
        {
            list.Add(new Point(point, dist(point)));
        }
    }

    #endregion

    #region Distributions

    IntervalDouble dist1(double p) => dist(p, M1, N1);
    double dist1_lower(double p) => dist1(p).Lower;
    double dist1_upper(double p) => dist1(p).Upper;

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

        return factor;
    }

    #endregion
}
