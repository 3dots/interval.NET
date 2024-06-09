#include "IntervalSystemStepWrapper.hpp"
namespace IntervalDotNET
{
	IntervalSystemStepWrapper::IntervalSystemStepWrapper(IntervalSystemStepWrapperFunc^ func)
	{
		Func = func;
	}

	Core::IntervalStruct IntervalSystemStepWrapper::intervalSystemStep(const double xLower, const double xUpper, const double t)
	{
		IntervalDouble^ computeResult = Func(xLower, xUpper, t);
		Core::IntervalStruct res;
		res.lower = computeResult->Lower;
		res.upper = computeResult->Upper;
		return res;
	}
}