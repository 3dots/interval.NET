#pragma once
#include "../interval.NET.Core/Core.hpp"

namespace Interval
{
	namespace NET {
		delegate double SystemFunc(double x, double t);

		public ref class OdeintWrapper
		{
		private:
			Core::System* m_SystemHolder;
			Core::Observer* m_ObserverHolder;

		public:
			OdeintWrapper(System::IntPtr systemFunc, System::IntPtr observerFunc);

			~OdeintWrapper()
			{
				this->!OdeintWrapper();
			}

			!OdeintWrapper()
			{
				if (m_ObserverHolder != nullptr) delete m_ObserverHolder;
				if (m_SystemHolder != nullptr) delete m_SystemHolder;
			}

			void IntegrateAdaptive(
				double absError,
				double relError,
				double startX,
				double startT,
				double endT,
				double dt
			);
		};
	}
}