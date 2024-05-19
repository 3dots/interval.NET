using IntervalDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace interval.NET;

public static class Odeint
{
    static readonly double ERR_TOLERANCE = 1E-12;

    public delegate double SystemFunc(double x, double t);
    public delegate void ObserverFunc(double x, double t);

    public static void IntegrateAdaptive(SystemFunc systemFunc, double startX, double startT, double endT, double dt, ObserverFunc observerFunc)
    {
        var w = new OdeintWrapper(Marshal.GetFunctionPointerForDelegate(systemFunc), Marshal.GetFunctionPointerForDelegate(observerFunc));
        w.IntegrateAdaptive(ERR_TOLERANCE, ERR_TOLERANCE, startX, startT, endT, dt);
    }
}
