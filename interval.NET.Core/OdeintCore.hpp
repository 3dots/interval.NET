#pragma once
namespace Core
{
	class OdeintCore
	{
	public:
		static void IntegrateAdaptive(
			double abs_error,
			double rel_error,
			void (*system)(double x, double& dxdt, double t), 
			double &start_state, 
			double start_time, 
			double end_time, 
			double dt, 
			void (*observer)(const double& x, double t)
		);

		static void Test(void (*system)(double x, double& dxdt, double t));
	};
}