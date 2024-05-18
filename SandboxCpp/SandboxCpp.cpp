#include <iostream>
#include <boost/numeric/odeint.hpp>

using namespace std;
using namespace boost::numeric::odeint;

void rhs(const double x, double& dxdt, const double t)
{
    dxdt = 3.0 / (2.0 * t * t) + x / (2.0 * t);
}

void write_cout(const double& x, const double t)
{
    cout << t << '\t' << x << endl;
}

// state_type = double
typedef runge_kutta_dopri5< double > stepper_type;

int main()
{
    double x = 0.0;
    integrate_adaptive(make_controlled(1E-12, 1E-12, stepper_type()),
        rhs, x, 1.0, 10.0, 0.1, write_cout);
}
