#include "IntervalDoubleCore.hpp"
#include "boost/numeric/interval.hpp"
using namespace boost::numeric;
using namespace interval_lib;

namespace Core
{
    class IntervalDoubleCore::IntervalDoubleCoreImpl {  
    public:
        const interval<double> intervalImp_;
        IntervalDoubleCoreImpl(double v) : intervalImp_(v) { }
        IntervalDoubleCoreImpl(double lower, double upper) : intervalImp_(lower, upper) { }
        IntervalDoubleCoreImpl(interval<double> i) : intervalImp_(i) { };
    };

    IntervalDoubleCore::IntervalDoubleCore(double v) {
        interval_ = new IntervalDoubleCoreImpl(v);
    }

    IntervalDoubleCore::IntervalDoubleCore(double lower, double upper)
    {
        interval_ = new IntervalDoubleCoreImpl(lower, upper);
    }

    double IntervalDoubleCore::upper() const { return interval_->intervalImp_.upper(); };
    double IntervalDoubleCore::lower() const { return interval_->intervalImp_.lower(); };

    IntervalDoubleCore IntervalDoubleCore::Add(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        interval<double> sum = ((*x).interval_)->intervalImp_ + ((*y).interval_)->intervalImp_;
        return IntervalDoubleCore(sum.lower(), sum.upper());
    }
}
