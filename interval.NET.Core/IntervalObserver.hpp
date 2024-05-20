#include "IntervalSystem.hpp"

#pragma once
namespace Core
{
	typedef void (*IntervalObserverStep)(const double xLower, const double xUpper, const double t);

	class IntervalObserver {
	private:
		IntervalObserverStep Func;

	public:
		IntervalObserver(IntervalObserverStep func) {
			Func = func;
		}

		void operator() (const interval_state_type &x, const double t)
		{
			Func(x[0], x[1], t);
		}
	};
}