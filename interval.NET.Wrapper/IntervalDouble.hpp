#pragma once
#include "ManagedObject.hpp"
#include "../interval.NET.Core/Core.hpp"
using namespace System;

namespace IntervalDotNET
{
    public ref class IntervalDouble : public ManagedObject<Core::IntervalDouble>
    {
    public:
        IntervalDouble(double v);
        IntervalDouble(double lower, double upper);

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

        static IntervalDouble^ Add(IntervalDouble^ x, IntervalDouble^ y);
    };
}