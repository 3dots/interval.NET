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

    IntervalDoubleWrapper^ IntervalDoubleWrapper::Subtract(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y)
    {
        Core::IntervalDoubleCore diff = Core::IntervalDoubleCore::Subtract(x->m_Instance, y->m_Instance);
        return gcnew IntervalDoubleWrapper(diff.lower(), diff.upper());
    }

    IntervalDoubleWrapper^ IntervalDoubleWrapper::Multiply(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y)
    {
        Core::IntervalDoubleCore product = Core::IntervalDoubleCore::Multiply(x->m_Instance, y->m_Instance);
        return gcnew IntervalDoubleWrapper(product.lower(), product.upper());
    }

    IntervalDoubleWrapper^ IntervalDoubleWrapper::Divide(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y)
    {
        Core::IntervalDoubleCore division = Core::IntervalDoubleCore::Divide(x->m_Instance, y->m_Instance);
        return gcnew IntervalDoubleWrapper(division.lower(), division.upper());
    }

    IntervalDoubleWrapper^ IntervalDoubleWrapper::Pow(IntervalDoubleWrapper^ x, int n)
    {
        Core::IntervalDoubleCore power = Core::IntervalDoubleCore::Pow(x->m_Instance, n);
        return gcnew IntervalDoubleWrapper(power.lower(), power.upper());
    }
}