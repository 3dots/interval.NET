#include "OdeintCore.hpp"
#include <boost/numeric/odeint.hpp>

using namespace boost::numeric::odeint;
typedef runge_kutta_dopri5< double > stepper_type;

namespace Core
{	
	void OdeintCore::IntegrateAdaptive(
		double abs_error,
		double rel_error,
		System system,
		double& start_state,
		double start_time,
		double end_time,
		double dt,
		Observer observer
	) {
		integrate_adaptive(make_controlled(abs_error, rel_error, stepper_type()), system, start_state, start_time, end_time, dt, observer);
	}

	typedef runge_kutta_dopri5< interval_state_type > interval_stepper_type;

	void OdeintCore::IntegrateAdaptive(
		double abs_error,
		double rel_error,
		IntervalSystem system,
		IntervalDoubleCore* start_interval,
		double start_time,
		double end_time,
		double dt,
		IntervalObserver observer
	) {
		interval_state_type start_state;
		start_state[0] = (*start_interval).lower();
		start_state[1] = (*start_interval).upper();

		integrate_adaptive(make_controlled(abs_error, rel_error, interval_stepper_type()), system, start_state, start_time, end_time, dt, observer);
	}
}