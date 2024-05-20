#include "IntervalDoubleCore.hpp"
#include <array>

#pragma once
namespace Core
{
	struct IntervalStruct {
		double lower;
		double upper;
	};

	typedef IntervalStruct (*IntervalSystemStep)(const double xLower, const double xUpper, const double t);
	typedef std::array< double, 2 > interval_state_type;

	class IntervalSystem {
	private:
		IntervalSystemStep Func;

	public:		
		IntervalSystem(IntervalSystemStep func) {
			Func = func;
		}

		void operator() (const interval_state_type &x, interval_state_type &dxdt, const double t)
		{
			IntervalStruct res = Func(x[0], x[1], t);
			dxdt[0] = res.lower;
			dxdt[1] = res.upper;
		}
	};
}