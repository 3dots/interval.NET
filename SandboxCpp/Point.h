#pragma once

#include "boost/numeric/interval.hpp"
using namespace boost::numeric;

namespace RSuccession
{
	struct Point
	{
		double x;
		interval<double> y;
	};
}
