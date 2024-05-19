#include <functional>

#pragma once
namespace Core
{
	typedef double (*SystemStep)(const double x, const double t);

	class System {
	public:
		SystemStep Func;

		System(SystemStep func) {
			Func = func;
		}

		void operator() (const double x, double& dxdt, const double t)
		{
			dxdt = Func(x, t);
		}
	};
}