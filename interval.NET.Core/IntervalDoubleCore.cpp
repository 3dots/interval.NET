#include "IntervalDoubleCore.hpp"
#include "boost/numeric/interval.hpp"
using namespace boost::numeric;
using namespace interval_lib;

namespace Core
{
    struct IntervalDoubleCore::IntervalDoubleCoreImpl {
        interval<double> interval;
        IntervalDoubleCoreImpl(double v) : interval(v) { }
        IntervalDoubleCoreImpl(double lower, double upper) : interval(lower, upper) { }
    };

    IntervalDoubleCore::IntervalDoubleCore(double v) : intervalImp_(std::make_unique<IntervalDoubleCoreImpl>(v)) { }

    IntervalDoubleCore::IntervalDoubleCore(double lower, double upper) : intervalImp_(std::make_unique<IntervalDoubleCoreImpl>(lower, upper)) { }

    IntervalDoubleCore::~IntervalDoubleCore() = default;

    double IntervalDoubleCore::upper() const { return intervalImp_->interval.upper(); };
    double IntervalDoubleCore::lower() const { return intervalImp_->interval.lower(); };

    double IntervalDoubleCore::Median(IntervalDoubleCore* x) 
    {
        return median((*x).intervalImp_->interval);
    }

    double IntervalDoubleCore::Width(IntervalDoubleCore* x)
    {
        return width((*x).intervalImp_->interval);
    }

    IntervalDoubleCore* IntervalDoubleCore::Add(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        interval<double> sum = (*x).intervalImp_->interval + (*y).intervalImp_->interval;
        return new IntervalDoubleCore(sum.lower(), sum.upper());
    }

    IntervalDoubleCore* IntervalDoubleCore::Subtract(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        interval<double> diff = (*x).intervalImp_->interval - (*y).intervalImp_->interval;
        return new IntervalDoubleCore(diff.lower(), diff.upper());
    }

    IntervalDoubleCore* IntervalDoubleCore::Multiply(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        interval<double> product = (*x).intervalImp_->interval * (*y).intervalImp_->interval;
        return new IntervalDoubleCore(product.lower(), product.upper());
    }

    IntervalDoubleCore* IntervalDoubleCore::Divide(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        interval<double> division = (*x).intervalImp_->interval / (*y).intervalImp_->interval;
        return new IntervalDoubleCore(division.lower(), division.upper());
    }

    bool IntervalDoubleCore::LessThan(IntervalDoubleCore* x, IntervalDoubleCore* y) 
    {
        return (*x).intervalImp_->interval < (*y).intervalImp_->interval;
    }

    bool IntervalDoubleCore::LessThanOrEqual(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        return (*x).intervalImp_->interval <= (*y).intervalImp_->interval;
    }

    bool IntervalDoubleCore::GreaterThan(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        return (*x).intervalImp_->interval > (*y).intervalImp_->interval;
    }

    bool IntervalDoubleCore::GreaterThanOrEqual(IntervalDoubleCore* x, IntervalDoubleCore* y)
    {
        return (*x).intervalImp_->interval >= (*y).intervalImp_->interval;
    }

    bool IntervalDoubleCore::Subset(IntervalDoubleCore* innerInterval, IntervalDoubleCore* outerInterval)
    {
        return subset((*innerInterval).intervalImp_->interval, (*outerInterval).intervalImp_->interval);
    }

    IntervalDoubleCore* IntervalDoubleCore::Pow(IntervalDoubleCore* x, int n)
    {        
        interval<double> power = pow((*x).intervalImp_->interval, n);
        return new IntervalDoubleCore(power.lower(), power.upper());
    }

    IntervalDoubleCore* IntervalDoubleCore::Sqrt(IntervalDoubleCore* x)
    {
        interval<double> power = sqrt((*x).intervalImp_->interval);
        return new IntervalDoubleCore(power.lower(), power.upper());
    }
}
