using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
namespace RenCon
{
    class SCalc : Interface1
    {
        public Common clc;
        public List<SData> sdt;
        public List<U> u_stg1;
        public List<SLoads> slds;
        public Matrix<double> R, Rinv, D, pl1;
        public Vector<double> Fg, Ff, eps1, eps2, orient;
        public Vector<double> E, v1, vb2, S1, Sb2, Sxx, Syy, Sxy;
        public CSV data;
        public double gb1, gb3, v, v01, v10, vv;
        public int fi, itrn, nlds, lcmin1, lcmin2, lcmax1, lcmax2;
        public int[] LC;
        public bool stg2;
        public Prop prp;
        public List<Materials> mat;
        public string Lim_st, Mes1, Mes2, mode;
        public SCalc(CSV sdat, string lmst, double g1, double g3, double vp, bool st2)
        {
            clc = new Common();
            data = sdat;
            slds = data.ReadSloads("sloads.csv");
            nlds = slds.Count;
            sdt = data.ReadSdata("sdata.csv");
            Lim_st = lmst;
            gb1 = g1;
            gb3 = g3;
            v = vp;
            stg2 = st2;
            Ff = DenseVector.Create(6, 0.0);
            mat = data.ReadMat();
            prp = new Prop(mat, gb1, gb3);
            pl1 = DenseMatrix.CreateDiagonal(3, 3, 1.0);
            eps1 = DenseVector.Create(sdt.Count, 0.0);
            eps2 = DenseVector.Create(sdt.Count, 0.0);
            E = DenseVector.Create(sdt.Count, 0.0);
            orient = DenseVector.Create(sdt.Count, 0.0);
            S1 = DenseVector.Create(sdt.Count, 0.0);
            Sb2 = DenseVector.Create(sdt.Count, 0.0);
            Sxx = DenseVector.Create(sdt.Count, 0.0);
            Syy = DenseVector.Create(sdt.Count, 0.0);
            Sxy = DenseVector.Create(sdt.Count, 0.0);
            Mes1 = " - Convergence ok";
            Mes2 = " - Solution ok";
        }
        public void Dat(double j)
        {
            v01 = v;
            v10 = v;
            vv = 1 - v01 * v10;
            prp.SData(sdt, Lim_st);
            double[,] RM = { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 2.0 } };
            R = DenseMatrix.OfArray(RM);
            double[,] RinvM = { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 0.5 } };
            Rinv = DenseMatrix.OfArray(RinvM);
            for (int i = 0; i < sdt.Count; i++)
            {
                E[i] = prp.E[i];
                { orient[i] = Math.PI * sdt[i].alpha / 180.0; }
            }
            v1 = DenseVector.Create(prp.nd, 1.0);
            vb2 = DenseVector.Create(prp.nd, 1.0);
            double[] MF = new double[6];
            if (Lim_st == "Second" && gb1 == 1.0)
            {
                MF[0] = slds[fi].Nxx2 / 1000 * j;
                MF[1] = slds[fi].Nyy2 / 1000 * j;
                MF[2] = slds[fi].Nxy2 / 1000 * j;
                MF[3] = slds[fi].Mxx2 / 1000 * j;
                MF[4] = slds[fi].Myy2 / 1000 * j;
                MF[5] = slds[fi].Mxy2 / 1000 * j;
            }
            else if (Lim_st == "Second" && gb1 == 0.9)
            {
                MF[0] = slds[fi].Nxx2l / 1000 * j;
                MF[1] = slds[fi].Nyy2l / 1000 * j;
                MF[2] = slds[fi].Nxy2l / 1000 * j;
                MF[3] = slds[fi].Mxx2l / 1000 * j;
                MF[4] = slds[fi].Myy2l / 1000 * j;
                MF[5] = slds[fi].Mxy2l / 1000 * j;
            }
            else
            {
                MF[0] = slds[fi].Nxx / 1000 * j;
                MF[1] = slds[fi].Nyy / 1000 * j;
                MF[2] = slds[fi].Nxy / 1000 * j;
                MF[3] = slds[fi].Mxx / 1000 * j;
                MF[4] = slds[fi].Myy / 1000 * j;
                MF[5] = slds[fi].Mxy / 1000 * j;
            }
            Fg = DenseVector.OfArray(MF);
        }
        public void Calc(Vector<double> u)
        {
            Matrix<double> pl, pl2, Cb, Cs;
            Vector<double> Sb, S_b, dc, STs, S_s, eps;
            double[,] CbM, CsM;
            double kRb, c, s, exx, eyy, gxy, ee1, ee2;
            for (int i = 0; i < sdt.Count; i++)
            {
                pl2 = DenseMatrix.CreateDiagonal(3, 3, sdt[i].Z);
                pl = pl1.Append(pl2);
                if (stg2 == true)
                {
                    u_stg1 = data.Read_U();
                    for (int j = 0; j < 6; j++)
                    { u[j] = u_stg1[j].u; }
                }
                eps = pl * u;
                if (prp.T[i] == "concrete")
                {
                    exx = eps[0];
                    eyy = eps[1];
                    gxy = eps[2];
                    ee1 = exx + eyy;
                    ee2 = exx - eyy;
                    double emax = ee1 / 2 + Math.Sqrt(Math.Pow(ee2 / 2, 2) + Math.Pow(gxy / 2, 2));
                    double emin = ee1 / 2 - Math.Sqrt(Math.Pow(ee2 / 2, 2) + Math.Pow(gxy / 2, 2));
                    eps1[i] = (emax + v01 * emin) / vv;
                    eps2[i] = (v10 * emax + emin) / vv;
                    orient[i] = 0.5 * Math.Atan2(gxy, ee2);
                    if (eps1[i] > 0.002)
                    { kRb = 1.0 / (0.8 + 100 * eps1[i]); }
                    else
                    { kRb = 1.0; }
                    S1[i] = clc.Sigma(eps1[i], i, prp, kRb);
                    Sb2[i] = clc.Sigma(eps2[i], i, prp, kRb);
                    Sb = DenseVector.OfArray(new double[] { S1[i], Sb2[i] });
                    if (eps1[i] != 0)
                    { v1[i] = S1[i] / E[i] / eps1[i]; }
                    else
                    { v1[i] = 1.0; }
                    if (eps2[i] != 0)
                    { vb2[i] = Sb2[i] / E[i] / eps2[i]; }
                    else
                    { vb2[i] = 1.0; }
                    c = Math.Cos(orient[i]);
                    s = Math.Sin(orient[i]);
                    CbM = new[,] { { c * c, s * s }, { s * s, c * c }, { s * c, -s * c } };
                    Cb = DenseMatrix.OfArray(CbM);
                    S_b = Cb * Sb;
                    Sxx[i] = S_b[0];
                    Syy[i] = S_b[1];
                    Sxy[i] = S_b[2];
                }
                else
                {
                    c = Math.Cos(orient[i]);
                    s = Math.Sin(orient[i]);
                    dc = DenseVector.OfArray(new[] { c * c, s * s, 2 * s * c });
                    eps1[i] = dc * eps;
                    S1[i] = clc.Sigma(eps1[i], i, prp, 1.0);
                    if (eps1[i] != 0)
                    { v1[i] = S1[i] / E[i] / eps1[i]; }
                    else
                    { v1[i] = 1.0; }
                    CsM = new[,] { { c * c, s * s }, { s * s, c * c }, { s * c, -s * c } };
                    Cs = DenseMatrix.OfArray(CsM);
                    STs = DenseVector.OfArray(new double[] { S1[i], 0.0 });
                    S_s = Cs * STs;
                    Sxx[i] = S_s[0];
                    Syy[i] = S_s[1];
                    Sxy[i] = S_s[2];
                }
            }
        }
        public Matrix<double> Qbar(double E1, double E2, int i)
        {
            double[,] QbM = new double[3, 3];
            if (E1 != 0.0)
            { v10 = E2 * v01 / E1; }
            else
            { v10 = 0.0; }
            double G01 = E[i] / (2 * (1 + v10));
            v01 = v10;
            vv = 1 - v01 * v10;
            QbM[0, 0] = E1 / vv;
            QbM[0, 1] = v01 * E2 / vv;
            QbM[1, 1] = E2 / vv;
            QbM[1, 0] = v10 * E2 / vv;
            QbM[2, 2] = G01;
            Matrix<double> T = Tcalc(orient[i]);
            Matrix<double> Qb = DenseMatrix.OfArray(QbM);
            Matrix<double> Q_bar = T.Inverse() * Qb * R * T * Rinv;
            return Q_bar;
        }
        public void Stiffness()
        {
            Matrix<double> Q_bar, Qs, T;
            double aa = 0.0;
            double bb = 0.0;
            double dd = 0.0;
            Matrix<double> a = DenseMatrix.Create(3, 3, 0.0);
            Matrix<double> b = DenseMatrix.Create(3, 3, 0.0);
            Matrix<double> d = DenseMatrix.Create(3, 3, 0.0);
            Qs = DenseMatrix.Create(3, 3, 0.0);
            for (int yy = 0; yy < 3; yy++)
            {
                for (int zz = 0; zz < 3; zz++)
                {
                    for (int xx = 0; xx < sdt.Count; xx++)
                    {
                        if (prp.T[xx] == "concrete")
                        {
                            Q_bar = Qbar(E[xx] * v1[xx], E[xx] * vb2[xx], xx);
                        }
                        else
                        {
                            Qs[0, 0] = E[xx] * v1[xx];
                            T = Tcalc(orient[xx]);
                            Q_bar = T.Inverse() * Qs * R * T * Rinv;
                        }
                        aa += Q_bar[yy, zz] * sdt[xx].A;
                        bb += Q_bar[yy, zz] * sdt[xx].Z * sdt[xx].A;
                        dd += Q_bar[yy, zz] * sdt[xx].Z * sdt[xx].Z * sdt[xx].A;
                    }
                    a[zz, yy] = aa;
                    b[zz, yy] = bb;
                    d[zz, yy] = dd;
                    aa = 0;
                    bb = 0;
                    dd = 0;
                }
            }
            Matrix<double> AB = a.Stack(b);
            Matrix<double> BD = b.Stack(d);
            D = AB.Append(BD);
        }
        public Matrix<double> Tcalc(double ornt)
        {
            double c = Math.Cos(ornt);
            double s = Math.Sin(ornt);
            double[,] TM = { { c * c, s * s, 2 * c * s },
                { s * s, c * c, -2 * c * s },
                { -c * s, c * s, c * c - s * s } };
            Matrix<double> T = DenseMatrix.OfArray(TM);
            return T;
        }
        public void Conv(Vector<double> u)
        {
            int i = 0;
            foreach (SData d in sdt)
            {
                Ff[0] += Sxx[i] * d.A;
                Ff[1] += Syy[i] * d.A;
                Ff[2] += Sxy[i] * d.A;
                Ff[3] += Sxx[i] * d.Z * d.A;
                Ff[4] += Syy[i] * d.Z * d.A;
                Ff[5] += Sxy[i] * d.Z * d.A;
                i++;
            }
            data.Converg(Fg * 1000, Ff * 1000, u, "convergence.csv", stg2);
        }
        public void CheckEps()
        {
            if (eps2.Min() < -0.0035 || eps1.Min() < -0.0035 || eps1.Max() > 0.025)
            {
                eps1 = DenseVector.Create(sdt.Count, 0.0);
                eps2 = DenseVector.Create(sdt.Count, 0.0);
                Mes1 = " - Convergence didn't riched !"; 
            }
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
                Vector<double> angle = orient * 180.0 / Math.PI;
                data.SResults(sdt, eps1, eps2, S1, Sb2, angle, "result.csv");
            }
            else if (mode == "L/C:divided")
            {
                int nd = 100;
                LC = new int[nd];
                double[] emin1 = new double[nd];
                double[] emin2 = new double[nd];
                double[] emax1 = new double[nd];
                double[] emax2 = new double[nd];
                double[] smin1 = new double[nd];
                double[] smin2 = new double[nd];
                double[] smax1 = new double[nd];
                double[] smax2 = new double[nd];
                for (int i = 0; i < nd; i++)
                {
                    fi = f;
                    LC[i] = i;
                    Iter(i * 1.0 / nd);
                    emin1[i] = eps1.Min();
                    smin1[i] = S1.Min();
                    emin2[i] = eps2.Min();
                    smin2[i] = Sb2.Min();
                    emax1[i] = eps1.Max();
                    smax1[i] = S1.Max();
                    emax2[i] = eps2.Max();
                    smax2[i] = Sb2.Max();
                }
                double e_min1 = emin1.Min();
                lcmin1 = Array.IndexOf(emin1, e_min1);
                double e_min2 = emin2.Min();
                lcmin2 = Array.IndexOf(emin2, e_min2);
                double e_max1 = emax1.Max();
                lcmax1 = Array.IndexOf(emax1, e_max1);
                double e_max2 = emax2.Max();
                lcmax2 = Array.IndexOf(emax2, e_max2);
                data.SResall(nd, LC, emin1, smin1, emin2, smin2, emax1, smax1, emax2, smax2, "result.csv");
            }
            else
            {
                LC = new int[nlds];
                double[] emin1 = new double[nlds];
                double[] emin2 = new double[nlds];
                double[] emax1 = new double[nlds];
                double[] emax2 = new double[nlds];
                double[] smin1 = new double[nlds];
                double[] smin2 = new double[nlds];
                double[] smax1 = new double[nlds];
                double[] smax2 = new double[nlds];
                for (int i = 0; i < nlds; i++)
                {
                    fi = i;
                    LC[i] = i;
                    Iter(1.0);
                    emin1[i] = eps1.Min();
                    smin1[i] = S1.Min();
                    emin2[i] = eps2.Min();
                    smin2[i] = Sb2.Min();
                    emax1[i] = eps1.Max();
                    smax1[i] = S1.Max();
                    emax2[i] = eps2.Max();
                    smax2[i] = Sb2.Max();
                }
                double e_min1 = emin1.Min();
                lcmin1 = Array.IndexOf(emin1, e_min1);
                double e_min2 = emin2.Min();
                lcmin2 = Array.IndexOf(emin2, e_min2);
                double e_max1 = emax1.Max();
                lcmax1 = Array.IndexOf(emax1, e_max1);
                double e_max2 = emax2.Max();
                lcmax2 = Array.IndexOf(emax2, e_max2);
                data.SResall(nlds, LC, emin1, smin1, emin2, smin2, emax1, smax1, emax2, smax2, "result.csv");
            }
        }
    }
}