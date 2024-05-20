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

    public static List<RPoint> IntegrateAdaptive(SystemFunc systemFunc, double startX, double startT, double endT, double dt)
    {
        List<RPoint> results = new();
        ObserverFunc observer = (double x, double t) => {
            //Console.WriteLine($"simple observer\t{x}\t{t}");
            IntervalDouble xInt = new IntervalDouble(x);
            IntervalDouble xLower = xInt - new IntervalDouble(ERR_TOLERANCE);
            IntervalDouble xUpper = xInt + new IntervalDouble(ERR_TOLERANCE);
            results.Add(new RPoint(t, new IntervalDouble(xLower.Lower, xUpper.Upper)));
        };

        var w = new OdeintWrapper(Marshal.GetFunctionPointerForDelegate(systemFunc), Marshal.GetFunctionPointerForDelegate(observer));
        w.IntegrateAdaptive(ERR_TOLERANCE, ERR_TOLERANCE, startX, startT, endT, dt);
        return results;
    }

    public delegate IntervalDouble IntervalSystemFunc(IntervalDouble x, double t);

    delegate void IntervalObserverFunc(double xLower, double xUpper, double t);
    //delegate IntervalDoubleWrapper IntervalSystemFuncWrapper(double xLower, double xUpper, double t);

    public static List<RPoint> IntegrateAdaptive(IntervalSystemFunc systemFunc, IntervalDouble startX, double startT, double endT, double dt)
    {
        List<RPoint> results = new();
        IntervalObserverFunc observer = (double xLower, double xUpper, double t) => { //IntervalObserverStep signature from InternalObserver.hpp
            Console.WriteLine($"observer\t{xLower}\t{xUpper}\t{t}");
            IntervalDouble xLowerInt = new IntervalDouble(xLower) - new IntervalDouble(ERR_TOLERANCE);
            IntervalDouble xUpperInt = new IntervalDouble(xUpper) + new IntervalDouble(ERR_TOLERANCE);
            results.Add(new RPoint(t, new IntervalDouble(xLowerInt.Lower, xUpperInt.Upper)));
        };

        IntervalSystemStepWrapperFunc system = (double xLower, double xUpper, double t) => {
            Console.WriteLine($"system\t{xLower}\t{xUpper}\t{t}");
            IntervalDouble x = xUpper >= xLower ? new IntervalDouble(xLower, xUpper) : new IntervalDouble(0); //odeint discards this data anyway. TODO: make sense of this.
            return systemFunc(x, t)._interval;
        };

        var w = new OdeintIntervalWrapper(Marshal.GetFunctionPointerForDelegate(observer));
        w.IntegrateAdaptive(system, ERR_TOLERANCE, ERR_TOLERANCE, startX._interval, startT, endT, dt);
        GC.KeepAlive(observer);
        return results;
    }
}
