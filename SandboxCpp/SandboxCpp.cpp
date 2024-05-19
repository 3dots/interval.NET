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

class TestSystem
{
public:
    TestSystem() {

    }

    void operator() (const double x, double& dxdt, const double t)
    {
        dxdt = dist(t, 1000, 500);
    }

    double dist(double p, int m, int n)
    {
        if (p <= 0 || p >= 1) return 0;

        double pI = p;
        double p1mp = 1 - pI;

        if (n > m)
        {
            int tempInt = m;
            m = n;
            n = tempInt;

            double tempI = pI;
            pI = p1mp;
            p1mp = tempI;
        }

        double factor = 1;
        for (int n1 = 1; n1 <= n; n1++)
        {
            double num = (m + n1) * pI * p1mp;
            double denom = n1;
            factor *= num / denom;
        }

        factor *= (m + n + 1) * pow(pI, m - n);

        return factor;
    }
};

class TestObserver
{
public:
    TestObserver() {

    }

    void operator() (const double x, const double t)
    {
        cout << t << '\t' << x << endl;
    }
};

// state_type = double
typedef runge_kutta_dopri5< double > stepper_type;

int main()
{
    double x = 0.0;
    TestSystem s;
    TestObserver o;
    integrate_adaptive(make_controlled(1E-12, 1E-12, stepper_type()),
        s, x, 0.0, 1.0, 0.001, o);
}
