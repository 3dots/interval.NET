#pragma once
#include "ManagedObject.hpp"
#include "../interval.NET.Core/Core.hpp"
using namespace System;

namespace IntervalDotNET
{
    public ref class IntervalDoubleWrapper : public ManagedObject<Core::IntervalDoubleCore>
    {
    public:
        IntervalDoubleWrapper(double v);
        IntervalDoubleWrapper(double lower, double upper);

        property double Upper
        {
        public:
            double get()
            {
                return m_Instance->upper();
            }
        }

        property double Lower
        {
        public:
            double get()
            {
                return m_Instance->lower();
            }
        }

        static IntervalDoubleWrapper^ Add(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static IntervalDoubleWrapper^ Subtract(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static IntervalDoubleWrapper^ Multiply(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static IntervalDoubleWrapper^ Divide(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static bool LessThan(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static bool LessThanOrEqual(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static bool GreaterThan(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static bool GreaterThanOrEqual(IntervalDoubleWrapper^ x, IntervalDoubleWrapper^ y);
        static bool Subset(IntervalDoubleWrapper^ innerInterval, IntervalDoubleWrapper^ outerInterval);
        static IntervalDoubleWrapper^ Pow(IntervalDoubleWrapper^ x, int n);
        static IntervalDoubleWrapper^ Sqrt(IntervalDoubleWrapper^ x);
    };
}