using MathNet.Numerics.LinearAlgebra;
using System;

namespace RenCon
{
    class Common
    {
        double S;
        public double Sigma(double e, int i, Prop prp, double kRb)
        {
            S = 0.0;
            double e_2 = prp.e_2[i];
            double e_0 = prp.e_0[i];
            double e_1 = prp.e_1[i];
            double e1 = prp.e1[i];
            double e0 = prp.e0[i];
            double e2 = prp.e2[i];
            double Rc = prp.Rc[i] * kRb;
            double S_1 = prp.S_1[i];
            double S1 = prp.S1[i];
            double Rt = prp.Rt[i];
            double E = prp.E[i];
            if (e <= e_0 && e >= e_2)
            { S = Rc; }
            else if (e > e_0 && e < e_1)
            { S = ((1 - S_1 / Rc) * (e - e_1) / (e_0 - e_1) + S_1 / Rc) * Rc; }
            else if (e >= e_1 && e < 0.0)
            { S = E * e; }
            else if (e > 0.0 && e <= e1 && Rt != 0.0)
            { S = E * e; }
            else if (e > e1 && e < e0 && Rt != 0.0)
            { S = ((1 - S1 / Rt) * (e - e1) / (e0 - e1) + S1 / Rt) * Rt; }
            else if (e >= e0 && e <= e2 && Rt != 0.0)
            { S = Rt; }
            else
            { S = 0.0; }
            return S;
        }
        public Vector<double> Solution(Matrix<double> D, Vector<double> Fg, ref string Mes2)
        {
            Vector<double> u = null;
            try
            {
                u = D.LU().Solve(Fg);
            }
            catch (Exception ex)
            {
                Mes2 = " - No Solution - " + ex.Message;
            }
            return u;
        }
    }
}
