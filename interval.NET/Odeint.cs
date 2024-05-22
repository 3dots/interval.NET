using IntervalDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace interval.NET;

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
        IntervalSystemFunc systemFunc, IntervalDouble startX, double startT, double endT, double dt)
        => IntegrateAdaptive(systemFunc, startX, startT, endT, dt, DEFAULT_ERR_TOLERANCE, DEFAULT_ERR_TOLERANCE);

    public static List<RPoint> IntegrateAdaptive(IntervalSystemFunc systemFunc, IntervalDouble startX, double startT, double endT, double dt, double absError, double relError)
    {
        List<RPoint> results = new();
        IntervalObserverFunc observer = (double xLower, double xUpper, double t) => { //IntervalObserverStep signature from InternalObserver.hpp
            //Console.WriteLine($"observer\t{xLower}\t{xUpper}\t{t}");
            IntervalDouble xLowerInt = new IntervalDouble(xLower) - new IntervalDouble(absError);
            IntervalDouble xUpperInt = new IntervalDouble(xUpper) + new IntervalDouble(absError);
            results.Add(new RPoint(t, new IntervalDouble(xLowerInt.Lower, xUpperInt.Upper)));
        };

        IntervalSystemStepWrapperFunc system = (double xLower, double xUpper, double t) => {
            //Console.WriteLine($"system\t{xLower}\t{xUpper}\t{t}");
            IntervalDouble x = xUpper >= xLower ? new IntervalDouble(xLower, xUpper) : new IntervalDouble(0); //odeint discards this data anyway. TODO: make sense of this.
            return systemFunc(x, t)._interval;
        };

        var w = new OdeintIntervalWrapper(Marshal.GetFunctionPointerForDelegate(observer));
        w.IntegrateAdaptive(system, absError, relError, startX._interval, startT, endT, dt);
        GC.KeepAlive(observer);
        GC.KeepAlive(system);
        return results;
    }

    #endregion

    #region Helpers

    static ObserverFunc ListObserver(List<RPoint> results, EnSystemPositivity sign, double errTol)
    {
        return (double x, double t) => {
            //Console.WriteLine($"{nameof(ListObserver)}\t{x}\t{t}");           
            results.Add(new RPoint(t, FromDouble(x, sign, errTol)));
        };
    }

    static ObserverFunc FinalObserver(RPoint endResult, EnSystemPositivity sign, double errTol)
    {
        return (double x, double t) => {
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

        if (sign == EnSystemPositivity.NonPositive && x >= -errTol)
        {
            upper = 0;
        }
        else
        {
            
        }

        IntervalDouble xUpper = xInt + new IntervalDouble(errTol);
        if ((sign == EnSystemPositivity.NonPositive || sign == EnSystemPositivity.Negative) && xUpper.Upper > 0)
            upper = 0;
        else
            upper = xUpper.Upper;

        return new IntervalDouble(lower, upper);
    }

    static IntervalObserverFunc IntervalListObserver(List<RPoint> results, EnSystemPositivity sign, double errTol)
    {
        return (double xLower, double xUpper, double t) => {
            //Console.WriteLine($"{nameof(IntervalListObserver)}\t{x}\t{t}");           
            results.Add(new RPoint(t, FromInterval(xLower, xUpper, sign, errTol)));
        };
    }

    static IntervalDouble FromInterval(double xLower, double xUpper, EnSystemPositivity sign, double errTol)
    {
        double lower;
        double upper;

        if (sign == EnSystemPositivity.NonNegative)
        {
            lower = 0;
        }
        else
        {
            IntervalDouble xLowerInt = new IntervalDouble(xLower);
            xLowerInt -= new IntervalDouble(errTol);
            lower = xLowerInt.Lower;
        }

        if (sign == EnSystemPositivity.NonPositive)
        {
            upper = 0;
        }
        else
        {
            IntervalDouble xUpper = xInt + new IntervalDouble(DEFAULT_ERR_TOLERANCE);
            upper = xUpper.Upper;
        }

        return new IntervalDouble(lower, upper);
    }

    #endregion 
}
