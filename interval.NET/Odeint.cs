using Interval.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Interval.NET;

public static class Odeint
{
    #region Fields

    static readonly double DEFAULT_ERR_TOLERANCE = 1E-12;

    #endregion

    #region Delegates

    public delegate double SystemFunc(double x, double t);
    delegate void ObserverFunc(double x, double t);

    public delegate IntervalDouble IntervalSystemFunc(IntervalDouble x, double t);
    delegate void IntervalObserverFunc(double xLower, double xUpper, double t);

    #endregion

    #region Public methods

    public static List<RPoint> IntegrateAdaptiveEvo(
        SystemFunc systemFunc, EnSystemPositivity sign, double startX, double startT, double endT, double dt)
        => IntegrateAdaptiveEvo(systemFunc, sign, startX, startT, endT, dt, DEFAULT_ERR_TOLERANCE, DEFAULT_ERR_TOLERANCE);

    public static List<RPoint> IntegrateAdaptiveEvo(
        SystemFunc systemFunc, EnSystemPositivity sign, double startX, double startT, double endT, double dt, double absError, double relError)
    {
        List<RPoint> results = new();
        ObserverFunc observer = ListObserver(results, sign, absError);

        var w = new OdeintWrapper(Marshal.GetFunctionPointerForDelegate(systemFunc), Marshal.GetFunctionPointerForDelegate(observer));
        w.IntegrateAdaptive(absError, relError, startX, startT, endT, dt);

        GC.KeepAlive(systemFunc);
        GC.KeepAlive(observer);
        return results;
    }

    public static IntervalDouble IntegrateAdaptive(
        SystemFunc systemFunc, EnSystemPositivity sign, double startX, double startT, double endT, double dt)
        => IntegrateAdaptive(systemFunc, sign, startX, startT, endT, dt, DEFAULT_ERR_TOLERANCE, DEFAULT_ERR_TOLERANCE);


    public static IntervalDouble IntegrateAdaptive(
        SystemFunc systemFunc, EnSystemPositivity sign, double startX, double startT, double endT, double dt, double absError, double relError)
    {
        RPoint result = new(endT, new IntervalDouble(double.NaN));
        ObserverFunc observer = FinalObserver(result, sign, absError);

        var w = new OdeintWrapper(Marshal.GetFunctionPointerForDelegate(systemFunc), Marshal.GetFunctionPointerForDelegate(observer));
        w.IntegrateAdaptive(absError, relError, startX, startT, endT, dt);

        GC.KeepAlive(systemFunc);
        GC.KeepAlive(observer);
        return result.Y;
    }

    public static List<RPoint> IntegrateAdaptiveEvo(
        IntervalSystemFunc systemFunc, EnSystemPositivity sign, IntervalDouble startX, double startT, double endT, double dt)
        => IntegrateAdaptiveEvo(systemFunc, sign, startX, startT, endT, dt, DEFAULT_ERR_TOLERANCE, DEFAULT_ERR_TOLERANCE);

    public static List<RPoint> IntegrateAdaptiveEvo(
        IntervalSystemFunc systemFunc,
        EnSystemPositivity sign,
        IntervalDouble startX,
        double startT,
        double endT,
        double dt,
        double absError,
        double relError)
    {
        List<RPoint> results = new();
        IntervalObserverFunc observer = IntervalListObserver(results, sign, absError);
        IntervalSystemStepWrapperFunc system = IntervalSystemWrapper(systemFunc);

        var w = new OdeintIntervalWrapper(Marshal.GetFunctionPointerForDelegate(observer));
        w.IntegrateAdaptive(system, absError, relError, startX, startT, endT, dt);
        GC.KeepAlive(observer);
        GC.KeepAlive(system);
        return results;
    }

    public static IntervalDouble IntegrateAdaptive(
        IntervalSystemFunc systemFunc,
        EnSystemPositivity sign,
        IntervalDouble startX,
        double startT,
        double endT,
        double dt)
        => IntegrateAdaptive(systemFunc, sign, startX, startT, endT, dt, DEFAULT_ERR_TOLERANCE, DEFAULT_ERR_TOLERANCE);

    public static IntervalDouble IntegrateAdaptive(
        IntervalSystemFunc systemFunc,
        EnSystemPositivity sign,
        IntervalDouble startX,
        double startT,
        double endT,
        double dt,
        double absError,
        double relError)
    {
        RPoint result = new(endT, new IntervalDouble(double.NaN));
        IntervalObserverFunc observer = IntervalFinalObserver(result, sign, absError);
        IntervalSystemStepWrapperFunc system = IntervalSystemWrapper(systemFunc);

        var w = new OdeintIntervalWrapper(Marshal.GetFunctionPointerForDelegate(observer));
        w.IntegrateAdaptive(system, absError, relError, startX, startT, endT, dt);
        GC.KeepAlive(observer);
        GC.KeepAlive(system);
        return result.Y;
    }

    #endregion

    #region Helpers

    static ObserverFunc ListObserver(List<RPoint> results, EnSystemPositivity sign, double errTol)
    {
        return (double x, double t) =>
        {
            //Console.WriteLine($"{nameof(ListObserver)}\t{x}\t{t}");           
            results.Add(new RPoint(t, FromDouble(x, sign, errTol)));
        };
    }

    static ObserverFunc FinalObserver(RPoint endResult, EnSystemPositivity sign, double errTol)
    {
        return (double x, double t) =>
        {
            //Console.WriteLine($"{nameof(FinalObserver)}\t{x}\t{t}");
            if (t == endResult.X)
            {
                endResult.Y = FromDouble(x, sign, errTol);
            }
        };
    }

    static IntervalDouble FromDouble(double x, EnSystemPositivity sign, double errTol)
    {
        double lower;
        double upper;
        IntervalDouble xInt = new IntervalDouble(x);

        IntervalDouble xLower = xInt - new IntervalDouble(errTol);
        if ((sign == EnSystemPositivity.NonNegative || sign == EnSystemPositivity.Positive) && xLower.Lower < 0)
            lower = 0;
        else
            lower = xLower.Lower;

        IntervalDouble xUpper = xInt + new IntervalDouble(errTol);
        if ((sign == EnSystemPositivity.NonPositive || sign == EnSystemPositivity.Negative) && xUpper.Upper > 0)
            upper = 0;
        else
            upper = xUpper.Upper;

        return new IntervalDouble(lower, upper);
    }

    static IntervalObserverFunc IntervalListObserver(List<RPoint> results, EnSystemPositivity sign, double errTol)
    {
        return (double xLower, double xUpper, double t) =>
        {
            //Console.WriteLine($"{nameof(IntervalListObserver)}\t{xLower}\t{xUpper}\t{t}");  
            results.Add(new RPoint(t, FromInterval(xLower, xUpper, sign, errTol)));
        };
    }

    static IntervalObserverFunc IntervalFinalObserver(RPoint endResult, EnSystemPositivity sign, double errTol)
    {
        return (double xLower, double xUpper, double t) =>
        {
            //Console.WriteLine($"{nameof(IntervalFinalObserver)}\t{xLower}\t{xUpper}\t{t}");
            if (t == endResult.X)
            {
                endResult.Y = FromInterval(xLower, xUpper, sign, errTol);
            }
        };
    }

    static IntervalDouble FromInterval(double xLower, double xUpper, EnSystemPositivity sign, double errTol)
    {
        double lower;
        double upper;

        IntervalDouble xLowerInt = new IntervalDouble(xLower) - new IntervalDouble(errTol);
        if ((sign == EnSystemPositivity.NonNegative || sign == EnSystemPositivity.Positive) && xLowerInt.Lower < 0)
            lower = 0;
        else
            lower = xLowerInt.Lower;

        IntervalDouble xUpperInt = new IntervalDouble(xUpper) + new IntervalDouble(errTol);
        if ((sign == EnSystemPositivity.NonPositive || sign == EnSystemPositivity.Negative) && xUpperInt.Upper > 0)
            upper = 0;
        else
            upper = xUpperInt.Upper;

        return new IntervalDouble(lower, upper);
    }

    static IntervalSystemStepWrapperFunc IntervalSystemWrapper(IntervalSystemFunc systemFunc)
    {
        return (double xLower, double xUpper, double t) =>
        {
            //Console.WriteLine($"system\t{xLower}\t{xUpper}\t{t}");
            
            if (xLower > xUpper)
            {
                int i = 1;
            }

            return systemFunc(new IntervalDouble(xLower, xUpper), t);
        };
    }

    #endregion

}
