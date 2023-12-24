#include "IntervalDoubleWrapper.hpp"
namespace IntervalDotNET
{
    IntervalDoubleWrapper::IntervalDoubleWrapper(double v) : ManagedObject(new Core::IntervalDoubleCore(v)) {

    }

    IntervalDoubleWrapper::IntervalDoubleWrapper(double lower, double upper) : ManagedObject(new Core::IntervalDoubleCore(lower, upper)) {

    }

    IntervalDoubleWrapper^ IntervalDoubleWrapper::Add(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y)
    {       
        Core::IntervalDoubleCore sum = Core::IntervalDoubleCore::Add(x->m_Instance, y->m_Instance);
        return gcnew IntervalDoubleWrapper(sum.lower(), sum.upper());
    }
}