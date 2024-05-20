#include "System.hpp"
#include "Observer.hpp"
#include "IntervalSystem.hpp"
#include "IntervalObserver.hpp"
#include "IntervalDoubleCore.hpp"

#pragma once
namespace Core
{
	class OdeintCore
	{
	public:
		static void IntegrateAdaptive(
			double abs_error,
			double rel_error,
			System system,
			double& start_state,
			double start_time,
			double end_time,
			double dt,
			Observer observer
		);

		static void IntegrateAdaptive(
			double abs_error,
			double rel_error,
			IntervalSystem system,
			IntervalDoubleCore* start_interval,
			double start_time,
			double end_time,
			double dt,
			IntervalObserver observer
		);
	};
}