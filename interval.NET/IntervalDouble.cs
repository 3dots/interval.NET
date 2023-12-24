using IntervalDotNET;

namespace interval.NET
{
    public class IntervalDouble
    {
        readonly IntervalDoubleWrapper _interval;

        public double Upper => _interval.Upper;
        public double Lower => _interval.Lower;

        public IntervalDouble(IntervalDoubleWrapper v)
        {
            _interval = v;
        }

        public IntervalDouble(double v) 
        {
            _interval = new IntervalDoubleWrapper(v);
        }

        public IntervalDouble(double lower, double upper)
        {
            _interval = new IntervalDoubleWrapper(lower, upper);
        }

        public static IntervalDouble operator +(IntervalDouble a, IntervalDouble b)
        {
            return new IntervalDouble(IntervalDoubleWrapper.Add(a._interval, b._interval));
        }
    }
}
