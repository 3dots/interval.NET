#include "OdeintIntervalWrapper.hpp"
namespace Interval
{
	namespace NET {
		OdeintIntervalWrapper::OdeintIntervalWrapper(System::IntPtr observerFunc) {
			Core::IntervalObserverStep observerStep = static_cast<Core::IntervalObserverStep>(observerFunc.ToPointer());
			m_ObserverHolder = new Core::IntervalObserver(observerStep);
		}

		void OdeintIntervalWrapper::IntegrateAdaptive(
			IntervalSystemStepWrapperFunc^ systemFunc,
			double absError,
			double relError,
			IntervalDouble^ startX,
			double startT,
			double endT,
			double dt
		) {
			IntervalSystemStepWrapper^ systemStepWrapper = gcnew IntervalSystemStepWrapper(systemFunc);
			IntervalSystemStepDelegate^ ManagedFunc = gcnew IntervalSystemStepDelegate(systemStepWrapper, &IntervalSystemStepWrapper::intervalSystemStep);
			IntPtr stubPointer = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(ManagedFunc);
			m_SystemHolder = new Core::IntervalSystem(static_cast<Core::IntervalSystemStep>(stubPointer.ToPointer()));

			Core::OdeintCore::IntegrateAdaptive(absError, relError, *m_SystemHolder, startX->GetInstance(), startT, endT, dt, *m_ObserverHolder);
		}
	}
}