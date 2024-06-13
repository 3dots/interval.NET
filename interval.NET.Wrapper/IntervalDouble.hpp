#pragma once
#include "ManagedObject.hpp"
#include "../interval.NET.Core/Core.hpp"
using namespace System;

namespace Interval
{
    namespace NET {
        public ref class IntervalDouble : public ManagedObject<Core::IntervalDoubleCore>
        {
        public:
            IntervalDouble(double v);
            IntervalDouble(double lower, double upper);
            IntervalDouble(Core::IntervalDoubleCore* core);

            virtual String^ ToString() override;

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

            property double Median
            {
            public:
                double get();
            }

            property double Error
            {
            public:
                double get();
            }

            static IntervalDouble^ operator+(IntervalDouble^ x, IntervalDouble^ y);
            static IntervalDouble^ operator-(IntervalDouble^ x, IntervalDouble^ y);
            static IntervalDouble^ operator*(IntervalDouble^ x, IntervalDouble^ y);
            static IntervalDouble^ operator/(IntervalDouble^ x, IntervalDouble^ y);

            static bool operator<(IntervalDouble^ x, IntervalDouble^ y);
            static bool operator>(IntervalDouble^ x, IntervalDouble^ y);
            static bool operator<=(IntervalDouble^ x, IntervalDouble^ y);
            static bool operator>=(IntervalDouble^ x, IntervalDouble^ y);

            static bool Subset(IntervalDouble^ innerInterval, IntervalDouble^ outerInterval);
            static IntervalDouble^ Pow(IntervalDouble^ x, int n);
            static IntervalDouble^ Sqrt(IntervalDouble^ x);
        };
    }
}