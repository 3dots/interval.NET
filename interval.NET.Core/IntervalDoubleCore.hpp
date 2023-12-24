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
            if (interval_ != nullptr)
            {
                delete interval_;
            }
        }

		double upper() const;
		double lower() const;

		static IntervalDoubleCore Add(IntervalDoubleCore* x, IntervalDoubleCore* y);
	};
}