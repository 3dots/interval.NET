#include <memory>

#pragma once
namespace Core
{
	class IntervalDoubleCore 
	{
	private:
		struct IntervalDoubleCoreImpl;
		std::unique_ptr<IntervalDoubleCoreImpl> intervalImp_;
	public:
		IntervalDoubleCore(double v);
		IntervalDoubleCore(double lower, double upper);

		~IntervalDoubleCore();

		double upper() const;
		double lower() const;

		static double Median(IntervalDoubleCore* x);
		static double Width(IntervalDoubleCore* x);

		static IntervalDoubleCore* Add(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static IntervalDoubleCore* Subtract(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static IntervalDoubleCore* Multiply(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static IntervalDoubleCore* Divide(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool LessThan(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool LessThanOrEqual(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool GreaterThan(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool GreaterThanOrEqual(IntervalDoubleCore* x, IntervalDoubleCore* y);
		static bool Subset(IntervalDoubleCore* innerInterval, IntervalDoubleCore* outerInterval);
		static IntervalDoubleCore* Pow(IntervalDoubleCore* x, int n);
		static IntervalDoubleCore* Sqrt(IntervalDoubleCore* x);
	};
}