#pragma once
namespace Core
{
	class IntervalDoubleCore 
	{
	private:
		class IntervalDoubleCoreImpl;
		const IntervalDoubleCoreImpl* interval_;
		IntervalDoubleCore(IntervalDoubleCoreImpl* interval);
	public:
		IntervalDoubleCore(double v);
		IntervalDoubleCore(double lower, double upper);
		
        ~IntervalDoubleCore()
        {
            if (interval_ != nullptr) delete interval_;
        }

		double upper() const;
		double lower() const;

		static IntervalDoubleCore Add(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static IntervalDoubleCore Subtract(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static IntervalDoubleCore Multiply(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static IntervalDoubleCore Divide(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool LessThan(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool LessThanOrEqual(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool GreaterThan(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool GreaterThanOrEqual(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool Subset(IntervalDoubleCore* innerInterval, IntervalDoubleCore* outerInterval);
		static IntervalDoubleCore Pow(IntervalDoubleCore* x, int n);
		static IntervalDoubleCore Sqrt(IntervalDoubleCore* x);
	};
}