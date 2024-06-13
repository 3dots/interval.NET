#include <iostream>
#include <boost/numeric/odeint.hpp>
#include <boost/numeric/interval.hpp>

using namespace std;
using namespace boost::numeric::odeint;

using namespace boost::numeric;
using namespace interval_lib;

typedef interval< double > intervalDouble;
typedef std::array< double, 2 > interval_state_type;

class TestSystem
{
private:
    intervalDouble dist(intervalDouble p, int m, int n)
    {
        if (p <= 0 || p >= 1) return 0;

        intervalDouble pI = p;
        intervalDouble p1mp = intervalDouble(1) - pI;

        if (n > m)
        {
            int tempInt = m;
            m = n;
            n = tempInt;

            intervalDouble tempI = pI;
            pI = p1mp;
            p1mp = tempI;
        }

        intervalDouble factor = 1;
        for (int n1 = 1; n1 <= n; n1++)
        {
            intervalDouble num = intervalDouble(m + n1) * pI * p1mp;
            intervalDouble denom = n1;
            factor *= num / denom;
        }

        factor *= intervalDouble(m + n + 1) * pow(pI, m - n);

        return factor;
    }

public:
    TestSystem() {

    }

    void operator() (const interval_state_type& x, interval_state_type& dxdt, const double t)
    {
        intervalDouble res = dist(intervalDouble(t), 1000, 500);
        dxdt[0] = res.lower();
        dxdt[1] = res.upper();
    }

    
};

class TestObserver
{
public:
    TestObserver() {

    }

    void operator() (const interval_state_type& x, const double t)
    {
        cout 
            << "t Median: " << t
            << "\tx Lower: " << x[0] << "\tUpper: " << x[1]
            << endl;
    }
};

// state_type = double
typedef runge_kutta_dopri5< double > stepper_type;

void print(intervalDouble& x) {
    cout
        << "Median: " << median(x) << "\tError: " << width(x) / 2 << endl;
}

int main()
{
    /*intervalDouble x = 1;
    intervalDouble y = 1e-35;

    intervalDouble sum = x + y;
    print(sum);*/

    /*interval_state_type x;
    x[0] = 0;
    x[1] = 0;
    TestSystem s;
    TestObserver o;
    integrate_adaptive(make_controlled(1e-12, 1e-12, stepper_type()),
        s, x, 0.0, 1.0, 0.001, o);*/
}
