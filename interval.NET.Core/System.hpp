#pragma once
namespace Core
{
	typedef double (*SystemStep)(const double x, const double t);

	class System {
	private:
		SystemStep Func;

	public:		
		System(SystemStep func) {
			Func = func;
		}

		void operator() (const double x, double& dxdt, const double t)
		{
			dxdt = Func(x, t);
		}
	};
}