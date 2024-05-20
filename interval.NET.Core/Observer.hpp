#pragma once
namespace Core
{
	typedef void (*ObserverStep)(const double x, const double t);

	class Observer {
	private:
		ObserverStep Func;

	public:
		Observer(ObserverStep func) {
			Func = func;
		}

		void operator() (const double x, const double t)
		{
			Func(x, t);
		}
	};
}