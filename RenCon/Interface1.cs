using MathNet.Numerics.LinearAlgebra;

namespace RenCon
{
    interface Interface1
    {
        void Start(int f, string m);
        void CheckEps();
        Vector<double> Iter(double j);
        void Dat(double j);
        void Stiffness();
        void Calc(Vector<double> u);
        void Conv(Vector<double> u);
    }
}
