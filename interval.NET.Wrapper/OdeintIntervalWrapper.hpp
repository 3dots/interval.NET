#pragma once
#include "../interval.NET.Core/Core.hpp"
#include "IntervalSystemStepWrapper.hpp"

namespace IntervalDotNET
{
	delegate double SystemFunc(double x, double t);

	public ref class OdeintIntervalWrapper
	{
	private:
		Core::IntervalSystem* m_SystemHolder;
		Core::IntervalObserver* m_ObserverHolder;

	public:
		OdeintIntervalWrapper(System::IntPtr observerFunc);

		~OdeintIntervalWrapper()
		{
			this->!OdeintIntervalWrapper();
		}

		!OdeintIntervalWrapper()
		{
			if (m_ObserverHolder != nullptr) delete m_ObserverHolder;
			if (m_SystemHolder != nullptr) delete m_SystemHolder;
		}

		void IntegrateAdaptive(
			IntervalSystemStepWrapperFunc^ systemFunc,
			double absError,
			double relError,
			IntervalDoubleWrapper^ startX,
			double startT,
			double endT,
			double dt
		);
	};
}