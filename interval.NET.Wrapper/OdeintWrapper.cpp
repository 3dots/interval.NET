#include "OdeintWrapper.hpp"
namespace IntervalDotNET
{
	void OdeintWrapper::IntegrateAdaptive(
		double absError,
		double relError
	) {
		//Core::OdeintCore::IntegrateAdaptive();
		//Core::OdeintCore::Test(this->system);
	}

	void OdeintWrapper::system(double x, double& dxdt, double t) {
		SystemFunc^ systemFunc = (SystemFunc^)SystemFuncHandle.Target;
		dxdt = systemFunc(x, t);
	}
}