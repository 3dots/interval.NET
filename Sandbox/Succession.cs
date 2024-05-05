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
    static readonly int POINTS_NO = 1000;

    static readonly int M1 = 1000;
    static readonly int N1 = 500;

    static readonly int M2 = 1000;
    static readonly int N2 = 300;

    public Succession()
    {

    }

    public void Run()
    {
        List<double> points = LinSpace(0, 1, POINTS_NO);
        List<IntervalDouble> distList = new();
        foreach (double point in points)
        {
            //string? format = "N15";
            //format = null;
            IntervalDouble dist = dist1(point);
            distList.Add(dist);
            //Console.WriteLine($"{point.ToString(format)} {dist}");
        }        

        Runtime.PythonDLL = "C:\\Users\\3dot\\AppData\\Local\\Programs\\Python\\Python312\\python312.dll";
        PythonEngine.Initialize();
        using (Py.GIL())
        {

            using (PyModule scope = Py.CreateScope())
            {
                scope.Import("matplotlib.pyplot", "plt");

                double[] pointsArr = points.ToArray();

                int count = distList.Count;
                double[] dist_lower = new double[count];
                double[] dist_upper = new double[count];

                for (int i = 0; i < distList.Count; i++)
                {
                    IntervalDouble dist = distList[i];
                    dist_lower[i] = dist.Lower;
                    dist_upper[i] = dist.Upper;
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

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }


            //dynamic np = Py.Import("numpy");
            //Console.WriteLine(np.cos(np.pi * 2));

            //dynamic sin = np.sin;
            //Console.WriteLine(sin(5));

            //double c = (double)(np.cos(5) + sin(5));
            //Console.WriteLine(c);

            //dynamic a = np.array(new List<float> { 1, 2, 3 });
            //Console.WriteLine(a.dtype);

            //dynamic b = np.array(new List<float> { 6, 5, 4 }, dtype: np.int32);
            //Console.WriteLine(b.dtype);

            //Console.WriteLine(a * b);          
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
}
