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

        public override string ToString()
        {
            return $"Median: {(_interval.Lower + _interval.Upper) / 2} Error: {(_interval.Upper - _interval.Lower) / 2} Lower: {_interval.Lower} Upper: {_interval.Upper}";
        }

        public static IntervalDouble operator +(IntervalDouble a, IntervalDouble b)
        {
            return new IntervalDouble(IntervalDoubleWrapper.Add(a._interval, b._interval));
        }

        public static IntervalDouble operator -(IntervalDouble a, IntervalDouble b)
        {
            return new IntervalDouble(IntervalDoubleWrapper.Subtract(a._interval, b._interval));
        }

        public static IntervalDouble operator *(IntervalDouble a, IntervalDouble b)
        {
            return new IntervalDouble(IntervalDoubleWrapper.Multiply(a._interval, b._interval));
        }

        public static IntervalDouble operator /(IntervalDouble a, IntervalDouble b)
        {
            return new IntervalDouble(IntervalDoubleWrapper.Divide(a._interval, b._interval));
        }

        public static IntervalDouble Pow(IntervalDouble x, int n)
        {
            return new IntervalDouble(IntervalDoubleWrapper.Pow(x._interval, n));
        }
    }
}
