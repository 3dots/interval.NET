using IntervalDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace interval.NET;

static class Odeint
{
    static readonly double ERR_TOLERANCE = 1E-12;

    public delegate double SystemFunc(double x, double t);

    static void IntegrateAdaptive(SystemFunc func)
    {
        var w = new OdeintWrapper(Marshal.GetFunctionPointerForDelegate(func));
        w.IntegrateAdaptive(ERR_TOLERANCE, ERR_TOLERANCE);
    }
}
