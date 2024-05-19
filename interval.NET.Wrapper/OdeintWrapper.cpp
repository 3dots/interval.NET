#include "OdeintWrapper.hpp"
namespace IntervalDotNET
{
	OdeintWrapper::OdeintWrapper(System::IntPtr systemFunc, System::IntPtr observerFunc) {
		Core::SystemStep systemStep = static_cast<Core::SystemStep>(systemFunc.ToPointer());
		Core::ObserverStep observerStep = static_cast<Core::ObserverStep>(observerFunc.ToPointer());
		m_SystemHolder = new Core::System(systemStep);
		m_ObserverHolder = new Core::Observer(observerStep);
	}

	void OdeintWrapper::IntegrateAdaptive(
		double absError,
		double relError,
		double startX,
		double startT,
		double endT,
		double dt
	) {
		Core::OdeintCore::IntegrateAdaptive(absError, relError, *m_SystemHolder, startX, startT, endT, dt, *m_ObserverHolder);
	}
}