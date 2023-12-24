#pragma once
namespace Core
{
	class IntervalDouble 
	{
	private:
		class IntervalDoubleImpl;
		const IntervalDoubleImpl* interval_;
		IntervalDouble(IntervalDoubleImpl* interval);
	public:
		IntervalDouble(double v);
		IntervalDouble(double lower, double upper);
		
        ~IntervalDouble()
        {
            if (interval_ != nullptr)
            {
                delete interval_;
            }
        }

		double upper() const;
		double lower() const;

		static IntervalDouble Add(IntervalDouble* x, IntervalDouble* y);
	};
}