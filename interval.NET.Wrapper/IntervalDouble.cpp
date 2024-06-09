#include "IntervalDouble.hpp"
namespace IntervalDotNET
{
	IntervalDouble::IntervalDouble(double v) : ManagedObject(new Core::IntervalDoubleCore(v)) { }

	IntervalDouble::IntervalDouble(double lower, double upper) : ManagedObject(new Core::IntervalDoubleCore(lower, upper)) { }

	IntervalDouble::IntervalDouble(Core::IntervalDoubleCore* core) : ManagedObject(core) { }

	double IntervalDouble::Median::get()
	{
		return Core::IntervalDoubleCore::Median(m_Instance);
	}

	double IntervalDouble::Error::get()
	{
		return Core::IntervalDoubleCore::Width(m_Instance) / 2;
	}

	String^ IntervalDouble::ToString()
	{
		return String::Format("Median: {0}, Error: {1}", Median, Error);
	}

	IntervalDouble^ IntervalDouble::operator+(IntervalDouble^ x, IntervalDouble^ y)
	{
		return gcnew IntervalDouble(Core::IntervalDoubleCore::Add(x->GetInstance(), y->GetInstance()));
	}

	IntervalDouble^ IntervalDouble::operator-(IntervalDouble^ x, IntervalDouble^ y)
	{
		return gcnew IntervalDouble(Core::IntervalDoubleCore::Subtract(x->GetInstance(), y->GetInstance()));
	}

	IntervalDouble^ IntervalDouble::operator*(IntervalDouble^ x, IntervalDouble^ y)
	{
		return gcnew IntervalDouble(Core::IntervalDoubleCore::Multiply(x->GetInstance(), y->GetInstance()));
	}

	IntervalDouble^ IntervalDouble::operator/(IntervalDouble^ x, IntervalDouble^ y)
	{
		return gcnew IntervalDouble(Core::IntervalDoubleCore::Divide(x->GetInstance(), y->GetInstance()));
	}

	bool IntervalDouble::operator<(IntervalDouble^ x, IntervalDouble^ y)
	{
		return Core::IntervalDoubleCore::LessThan(x->GetInstance(), y->GetInstance());
	}

	bool IntervalDouble::operator<=(IntervalDouble^ x, IntervalDouble^ y)
	{
		return Core::IntervalDoubleCore::LessThanOrEqual(x->GetInstance(), y->GetInstance());
	}

	bool IntervalDouble::operator>(IntervalDouble^ x, IntervalDouble^ y)
	{
		return Core::IntervalDoubleCore::GreaterThan(x->GetInstance(), y->GetInstance());
	}

	bool IntervalDouble::operator>=(IntervalDouble^ x, IntervalDouble^ y)
	{
		return Core::IntervalDoubleCore::GreaterThanOrEqual(x->GetInstance(), y->GetInstance());
	}	

	bool IntervalDouble::Subset(IntervalDouble^ innerInterval, IntervalDouble^ outerInterval)
	{
		return Core::IntervalDoubleCore::Subset(innerInterval->m_Instance, outerInterval->m_Instance);
	}

	IntervalDouble^ IntervalDouble::Pow(IntervalDouble^ x, int n)
	{
		return gcnew IntervalDouble(Core::IntervalDoubleCore::Pow(x->m_Instance, n));
	}

	IntervalDouble^ IntervalDouble::Sqrt(IntervalDouble^ x)
	{
		return gcnew IntervalDouble(Core::IntervalDoubleCore::Sqrt(x->m_Instance));
	}
}