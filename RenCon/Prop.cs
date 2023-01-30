using System.Collections.Generic;
namespace RenCon
{
    class Prop
    {
        public List<Materials> mat;
        public double[] e_2;
        public double[] e_0;
        public double[] e_1;
        public double[] e1;
        public double[] e0;
        public double[] e2;
        public double[] Rc;
        public double[] S_1;
        public double[] S1;
        public double[] Rt;
        public double[] E;
        public string[] T;
        public double gb1, gb3;
        public int nd;
        public Prop(List<Materials> mt, double g1, double g3)
        {
            mat = mt;
            gb1 = g1;
            gb3 = g3;
        }
        public void FData(List<FData> fdt, string Lim_st)
        {
            nd = fdt.Count;
            Prps(nd);
            int i = 0;
            foreach (FData d in fdt)
            {
                foreach (Materials m in mat)
                    if (m.Grade == d.Grade)
                    {
                        Sel(m, Lim_st, i);
                    }
                i++;
            }
        }
        public void SData(List<SData> sdt, string Lim_st)
        {
            nd = sdt.Count;
            Prps(nd);
            int i = 0;
            foreach (SData d in sdt)
            {
                foreach (Materials m in mat)
                    if (m.Grade == d.Grade)
                    {
                        Sel(m, Lim_st, i);
                    }
                i++;
            }
        }
        public void Prps(int nd)
        {
            e_2 = new double[nd];
            e_0 = new double[nd];
            e_1 = new double[nd];
            e1 = new double[nd];
            e0 = new double[nd];
            e2 = new double[nd];
            Rc = new double[nd];
            S_1 = new double[nd];
            S1 = new double[nd];
            Rt = new double[nd];
            E = new double[nd];
            T = new string[nd];
        }
        public void Sel(Materials m, string Lim_st, int i)
        {
            e_2[i] = m.eps2;
            e_0[i] = m.eps0;
            e_1[i] = m.eps1;
            e1[i] = m.epst1;
            e0[i] = m.epst0;
            e2[i] = m.epst2;
            E[i] = m.E;
            T[i] = m.T;
            S_1[i] = m.Sc1;
            S1[i] = m.St1;
            Rc[i] = m.Rc;
            Rt[i] = m.Rt;
            if (Lim_st == "Second" && m.T == "concrete")
            {
                Rc[i] = m.Rc * 1.3;
                Rt[i] = m.Rt * 1.5;
            }
            else if (Lim_st == "Second" && m.T == "rebar")
            {
                Rt[i] = m.Rt * 1.15;
            }
            if (Lim_st == "First" && m.T == "concrete")
            {
                Rc[i] = m.Rc * gb1 * gb3;
                S1[i] = 0.0;
                Rt[i] = 0.0;
            }
        }
    }
}