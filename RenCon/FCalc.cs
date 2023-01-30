using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RenCon
{
    class FCalc : Interface1
    {
        public Common clc;
        public List<FData> fdt;
        public List<FLoads> flds;
        public List<U> u_stg1;
        public Matrix<double> D;
        public Vector<double> Fg, Ff, v, eps, sig;
        public CSV data;
        public double gb1, gb3;
        public int fi, itrn, nlds, lcmin, lcmax;
        public int[] LC;
        public bool stg2;
        public Prop prp;
        public List<Materials> mat;
        public string Lim_st, Mes1, Mes2, mode;
        public FCalc(CSV fdat, string lmst, double g1, double g3, bool st2)
        {
            clc = new Common();
            data = fdat;
            flds = data.ReadFloads("floads.csv");
            nlds = flds.Count;
            fdt = data.ReadFdata("fdata.csv");
            Lim_st = lmst;
            gb1 = g1;
            gb3 = g3;
            stg2 = st2;
            mat = data.ReadMat();
            prp = new Prop(mat, gb1, gb3);
            Mes1 = " - Convergence ok";
            Mes2 = " - Solution ok";
        }
        public void Dat(double j)
        {
            prp.FData(fdt, Lim_st);
            v = DenseVector.Create(prp.nd, 1.0);
            eps = DenseVector.Create(prp.nd, 0.0);
            sig = DenseVector.Create(prp.nd, 0.0);
            Ff = DenseVector.Create(3, 0.0);
            double[] MF = new double[3];
            if (Lim_st == "Second" && gb1 == 1.0)
            {
                MF[0] = flds[fi].Mx2 / 1000 * j;
                MF[1] = flds[fi].My2 / 1000 * j;
                MF[2] = flds[fi].N2 / 1000 * j;
            }
            else if (Lim_st == "Second" && gb1 == 0.9)
            {
                MF[0] = flds[fi].Mx2l / 1000 * j;
                MF[1] = flds[fi].My2l / 1000 * j;
                MF[2] = flds[fi].N2l / 1000 * j;
            }
            else
            {
                MF[0] = flds[fi].Mx / 1000 * j;
                MF[1] = flds[fi].My / 1000 * j;
                MF[2] = flds[fi].N / 1000 * j;
            }
            Fg = DenseVector.OfArray(MF);
        }
        public void Calc(Vector<double> u)
        {
            int i = 0;
            foreach (FData d in fdt)
            {
                if (stg2 == true)
                {
                    u_stg1 = data.Read_U();
                    eps[i] = u[2] + u_stg1[2].u + (u[0] + u_stg1[0].u) * d.Zx + (u[1] + u_stg1[1].u) * d.Zy;
                }
                else
                { eps[i] = u[2] + u[0] * d.Zx + u[1] * d.Zy; }
                sig[i] = clc.Sigma(eps[i], i, prp, 1.0);
                if (eps[i] != 0.0)
                { v[i] = sig[i] / prp.E[i] / eps[i]; }
                else
                { v[i] = 1.0; }
                i++;
            }
        }
        public void Stiffness()
        {
            D = DenseMatrix.Create(3, 3, 0);
            for (int i = 0; i < prp.nd; i++)
            {
                D[0, 0] += fdt[i].A * fdt[i].Zx * fdt[i].Zx * prp.E[i] * v[i];
                D[0, 1] += fdt[i].A * fdt[i].Zx * fdt[i].Zy * prp.E[i] * v[i];
                D[0, 2] += fdt[i].A * fdt[i].Zx * prp.E[i] * v[i];
                D[1, 0] += fdt[i].A * fdt[i].Zx * fdt[i].Zy * prp.E[i] * v[i];
                D[1, 1] += fdt[i].A * fdt[i].Zy * fdt[i].Zy * prp.E[i] * v[i];
                D[1, 2] += fdt[i].A * fdt[i].Zy * prp.E[i] * v[i];
                D[2, 2] += fdt[i].A * prp.E[i] * v[i];
                D[2, 0] += fdt[i].A * fdt[i].Zx * prp.E[i] * v[i];
                D[2, 1] += fdt[i].A * fdt[i].Zy * prp.E[i] * v[i];
            }
        }
        public void Conv(Vector<double> u)
        {
            int i = 0;
            foreach (FData d in fdt)
            {
                Ff[0] += sig[i] * d.Zx * fdt[i].A;
                Ff[1] += sig[i] * d.Zy * fdt[i].A;
                Ff[2] += sig[i] * fdt[i].A;
                i++;
            }
            data.Converg(Fg * 1000, Ff * 1000, u, "convergence.csv", stg2);
        }
        public void CheckEps()
        {
            if (eps.Min() < -0.0035 || eps.Max() > 0.025)
            { Mes1 = " - Convergence didn't riched !"; }
            else
            { Mes1 = " - Convergence ok"; }
        }
        public Vector<double> Iter(double j)
        {
            Vector<double> u, u_f;
            double acc = 0.0000001;
            double du = 0.0001;
            itrn = 0;
            Dat(j);
            Stiffness();
            u = clc.Solution(D, Fg, ref Mes2);
            Calc(u);
            while (du >= acc)
            {
                itrn++;
                CheckEps();
                if (Mes1 == " - Convergence didn't riched !")
                { break; }
                Stiffness();
                u_f = clc.Solution(D, Fg, ref Mes2);
                Calc(u_f);
                du = (u - u_f).AbsoluteMaximum();
                u = u_f;
            }
            return u;
        }
        public void Start(int f, string m)
        {
            mode = m;
            if (mode == "L/C:selected")
            {
                fi = f;
                Vector<double> u = Iter(1.0);
                Conv(u);
                data.FResults(fdt, eps, sig, "result.csv");
            }
            else if (mode == "L/C:divided")
            {
                int nd = 100;
                LC = new int[nd];
                double[] emin = new double[nd];
                double[] emax = new double[nd];
                double[] smin = new double[nd];
                double[] smax = new double[nd];
                for (int i = 0; i < nd; i++)
                {
                    fi = f;
                    LC[i] = i;
                    Iter(i*1.0/ nd);
                    emin[i] = eps.Min();
                    emax[i] = eps.Max();
                    smin[i] = sig.Min();
                    smax[i] = sig.Max();
                }
                double e_min = emin.Min();
                lcmin = Array.IndexOf(emin, e_min);
                double e_max = emax.Max();
                lcmax = Array.IndexOf(emax, e_max);
                data.FResall(nd, LC, emin, smin, emax, smax, "result.csv");
            }
            else
            {
                LC = new int[nlds];
                double[] emin = new double[nlds];
                double[] emax = new double[nlds];
                double[] smin = new double[nlds];
                double[] smax = new double[nlds];
                for (int i = 0; i < nlds; i++)
                {
                    fi = i;
                    LC[i] = i;
                    Iter(1.0);
                    emin[i] = eps.Min();
                    emax[i] = eps.Max();
                    smin[i] = sig.Min();
                    smax[i] = sig.Max();
                }
                double e_min = emin.Min();
                lcmin = Array.IndexOf(emin, e_min);
                double e_max = emax.Max();
                lcmax = Array.IndexOf(emax, e_max);
                data.FResall(nlds, LC, emin, smin, emax, smax, "result.csv");
            }
        }
    }
}