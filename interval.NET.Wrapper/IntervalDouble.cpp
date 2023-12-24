#include "IntervalDouble.hpp"
namespace IntervalDotNET
{
    IntervalDouble::IntervalDouble(double v) : ManagedObject(new Core::IntervalDouble(v)) {

    }

    IntervalDouble::IntervalDouble(double lower, double upper) : ManagedObject(new Core::IntervalDouble(lower, upper)) {

    }

    IntervalDouble^ IntervalDouble::Add(IntervalDouble^ x, IntervalDouble^ y)
    {       
        Core::IntervalDouble sum = Core::IntervalDouble::Add(x->m_Instance, y->m_Instance);
        return gcnew IntervalDouble(sum.lower(), sum.upper());
    }
}