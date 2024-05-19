#pragma once
#include "../interval.NET.Core/Core.hpp"
using System::Runtime::InteropServices::GCHandle;

namespace IntervalDotNET
{
	delegate double SystemFunc(double x, double t);

	public ref class OdeintWrapper
	{
	private:
		GCHandle SystemFuncHandle;

	public:
		OdeintWrapper(System::IntPtr systemFunc) {
			SystemFuncHandle = GCHandle::FromIntPtr(systemFunc);
		}

		~OdeintWrapper()
		{
			this->!OdeintWrapper();
		}

		!OdeintWrapper()
		{
			delete SystemFuncHandle;
		}

		void IntegrateAdaptive(
			double absError, 
			double relError
		);

		void system(double x, double& dxdt, double t);
	};
}