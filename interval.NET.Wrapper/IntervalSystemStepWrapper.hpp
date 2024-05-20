#pragma once
#include "../interval.NET.Core/Core.hpp"
#include "IntervalDoubleWrapper.hpp"


namespace IntervalDotNET
{
	public delegate IntervalDoubleWrapper^ IntervalSystemStepWrapperFunc(const double xLower, const double xUpper, const double t);

	delegate Core::IntervalStruct IntervalSystemStepDelegate(const double xLower, const double xUpper, const double t);

	public ref class IntervalSystemStepWrapper
	{
	private:
		IntervalSystemStepWrapperFunc^ Func;
	public:
		IntervalSystemStepWrapper(IntervalSystemStepWrapperFunc^ func);

		Core::IntervalStruct intervalSystemStep(const double xLower, const double xUpper, const double t);
	};
}