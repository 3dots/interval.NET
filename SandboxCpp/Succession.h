#pragma once

#include <vector>
#include "boost/numeric/interval.hpp"
#include "Point.h"

namespace RSuccession
{
	class Succession
	{
	private:
		static const int POINTS_NO_INITIAL = 1000;
		static const int POINTS_NO_FOCUS = 1000;
		const double POS_THRESHOLD = 0.001;

		const int M1 = 1000;
		const int N1 = 500;

		const int M2 = 1000;
		const int N2 = 300;

		std::vector<Point> Data;

	public:
		Succession();
		void run();

	private:

	};

}
