#include "IntervalDouble.hpp"
#include "boost/numeric/interval.hpp"
using namespace boost::numeric;
using namespace interval_lib;

namespace Core
{
    class IntervalDouble::IntervalDoubleImpl {  
    public:
        const interval<double> intervalImp_;
        IntervalDoubleImpl(double v) : intervalImp_(v) { }
        IntervalDoubleImpl(double lower, double upper) : intervalImp_(lower, upper) { }
        IntervalDoubleImpl(interval<double> i) : intervalImp_(i) { };
    };

    IntervalDouble::IntervalDouble(double v) {
        interval_ = new IntervalDoubleImpl(v);
    }

    IntervalDouble::IntervalDouble(double lower, double upper)
    {
        interval_ = new IntervalDoubleImpl(lower, upper);
    }

    double IntervalDouble::upper() const { return interval_->intervalImp_.upper(); };
    double IntervalDouble::lower() const { return interval_->intervalImp_.lower(); };

    IntervalDouble IntervalDouble::Add(IntervalDouble* x, IntervalDouble* y)
    {
        interval<double> sum = ((*x).interval_)->intervalImp_ + ((*y).interval_)->intervalImp_;
        return IntervalDouble(sum.lower(), sum.upper());
    }
}
