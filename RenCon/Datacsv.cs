using CsvHelper;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
namespace RenCon
{
    public class Materials
    {
        public string Grade { get; set; }
        public double eps2 { get; set; }
        public double eps0 { get; set; }
        public double eps1 { get; set; }
        public double epst1 { get; set; }
        public double epst0 { get; set; }
        public double epst2 { get; set; }
        public double Rc { get; set; }
        public double Sc1 { get; set; }
        public double St1 { get; set; }
        public double Rt { get; set; }
        public double E { get; set; }
        public string T { get; set; }
    }
    public class FData
    {
        public string Grade { get; set; }
        public double Zx { get; set; }
        public double Zy { get; set; }
        public double A { get; set; }
    }
    public class FLoads
    {
        public double Mx { get; set; }
        public double My { get; set; }
        public double N { get; set; }
        public double Mx2 { get; set; }
        public double My2 { get; set; }
        public double N2 { get; set; }
        public double Mx2l { get; set; }
        public double My2l { get; set; }
        public double N2l { get; set; }
    }
    public class FResults
    {
        public string Grade { get; set; }
        public double Zx { get; set; }
        public double Zy { get; set; }
        public double Strain { get; set; }
        public double Stress { get; set; }
        public double Area { get; set; }
    }
    public class FResall
    {
        public double LC { get; set; }
        public double Epsmin { get; set; }
        public double Sigmin { get; set; }
        public double Epsmax { get; set; }
        public double Sigmax { get; set; }
    }
    public class SResults
    {
        public string Grade { get; set; }
        public double Z { get; set; }
        public double Strain1 { get; set; }
        public double Strain2 { get; set; }
        public double Stress1 { get; set; }
        public double Stress2 { get; set; }
        public double Angle { get; set; }
        public double Area { get; set; }
    }
    public class SResall
    {
        public double LC { get; set; }
        public double Eps1min { get; set; }
        public double Sig1min { get; set; }
        public double Eps2min { get; set; }
        public double Sig2min { get; set; }
        public double Eps1max { get; set; }
        public double Sig1max { get; set; }
        public double Eps2max { get; set; }
        public double Sig2max { get; set; }
    }
    public class Conv
    {
        public double Fgiven { get; set; }
        public double Ffound { get; set; }
        public double u { get; set; }
    }
    public class U
    {
        public double u { get; set; }
    }
    public class SData
    {
        public string Grade { get; set; }
        public double Z { get; set; }
        public double A { get; set; }
        public int n { get; set; }
        public double alpha { get; set; }
    }
    public class SLoads
    {
        public double Nxx { get; set; }
        public double Nyy { get; set; }
        public double Nxy { get; set; }
        public double Mxx { get; set; }
        public double Myy { get; set; }
        public double Mxy { get; set; }
        public double Nxx2 { get; set; }
        public double Nyy2 { get; set; }
        public double Nxy2 { get; set; }
        public double Mxx2 { get; set; }
        public double Myy2 { get; set; }
        public double Mxy2 { get; set; }
        public double Nxx2l { get; set; }
        public double Nyy2l { get; set; }
        public double Nxy2l { get; set; }
        public double Mxx2l { get; set; }
        public double Myy2l { get; set; }
        public double Mxy2l { get; set; }
    }
    public class CSV
    {
        public string Path = "C:\\data\\";
        public string Dlm = ";";
        public List<Materials> ReadMat()
        {
            using (var reader = new StreamReader(Path + "materials.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<Materials> records = csv.GetRecords<Materials>().ToList();
                return records;
            }
        }
        public List<FData> ReadFdata(string filename)
        {
            using (var reader = new StreamReader(Path + filename))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<FData> records = csv.GetRecords<FData>().ToList();
                return records;
            }
        }
        public List<FLoads> ReadFloads(string filename)
        {
            using (var reader = new StreamReader(Path + filename))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<FLoads> records = csv.GetRecords<FLoads>().ToList();
                return records;
            }
        }
        public List<SData> ReadSdata(string filename)
        {
            using (var reader = new StreamReader(Path + filename))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<SData> records = csv.GetRecords<SData>().ToList();
                return records;
            }
        }
        public List<SLoads> ReadSloads(string filename)
        {
            using (var reader = new StreamReader(Path + filename))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<SLoads> records = csv.GetRecords<SLoads>().ToList();
                return records;
            }
        }
        public void FResults(List<FData> fdt, Vector<double> eps, Vector<double> sig, string filename)
        {
            var records = new List<FResults>
            {
                new FResults { Grade=fdt[0].Grade,Zx=fdt[0].Zx,Zy=fdt[0].Zy,Strain=eps[0],Stress=sig[0],
                    Area=fdt[0].A},

            };
            for (int i = 1; i < fdt.Count(); i++)
            {
                records.Add(new FResults()
                {
                    Grade = fdt[i].Grade,
                    Zx = fdt[i].Zx,
                    Zy = fdt[i].Zy,
                    Strain = eps[i],
                    Stress = sig[i],
                    Area = fdt[i].A
                });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
        }
        public void Fdata(string[] Gr, double[] zx, double[] zy, double[] a, string filename)
        {
            var records = new List<FData>
            {
                new FData { Grade=Gr[0], Zx=zx[0], Zy=zy[0], A=a[0]},
            };
            for (int i = 1; i < Gr.Count(); i++)
            {
                records.Add(new FData()
                {
                    Grade = Gr[i],
                    Zx = zx[i],
                    Zy = zy[i],
                    A = a[i],
                });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
        }
        public void FResall(int nlds, int[] LC, double[] emin, double[] smin, double[] emax, double[] smax,
            string filename)
        {
            var records = new List<FResall>
            {
                new FResall { LC=LC[0], Epsmin=emin[0], Sigmin=smin[0], Epsmax=emax[0], Sigmax=smax[0] },

            };
            for (int i = 1; i < nlds; i++)
            {
                records.Add(new FResall()
                {
                    LC = LC[i],
                    Epsmin = emin[i],
                    Sigmin = smin[i],
                    Epsmax = emax[i],
                    Sigmax = smax[i]
                });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
        }
        public void SResults(List<SData> sdt, Vector<double> eps1, Vector<double> eps2, Vector<double> S1,
            Vector<double> Sb2, Vector<double> Angle, string filename)
        {
            var records = new List<SResults>
            {
                new SResults { Grade=sdt[0].Grade,Z=sdt[0].Z,Strain1=eps1[0],Strain2=eps2[0],Stress1=S1[0],
                    Stress2=Sb2[0],Angle=Angle[0],Area=sdt[0].A },

            };
            for (int i = 1; i < sdt.Count(); i++)
            {
                records.Add(new SResults()
                {
                    Grade = sdt[i].Grade,
                    Z = sdt[i].Z,
                    Strain1 = eps1[i],
                    Strain2 = eps2[i],
                    Stress1 = S1[i],
                    Stress2 = Sb2[i],
                    Angle = Angle[i],
                    Area = sdt[i].A
                });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
        }
        public void Sdata(string[] Gr, double[] z, double[] a, int[] ni, double[] al, string filename)
        {
            var records = new List<SData>
            {
                new SData { Grade=Gr[0], Z=z[0], A=a[0], n=ni[0], alpha=al[0]},
            };
            for (int i = 1; i < Gr.Count(); i++)
            {
                records.Add(new SData()
                {
                    Grade = Gr[i],
                    Z = z[i],
                    A = a[i],
                    n = ni[i],
                    alpha = al[i],
                });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
        }
        public void SResall(int nlds, int[] LC, double[] emin1, double[] smin1, double[] emin2, double[] smin2,
            double[] emax1, double[] smax1, double[] emax2, double[] smax2, string filename)
        {
            var records = new List<SResall>
            {
                new SResall { LC=LC[0], Eps1min=emin1[0], Sig1min=smin1[0], Eps2min=emin2[0], Sig2min=smin2[0],
                    Eps1max=emax1[0], Sig1max=smax1[0], Eps2max=emax2[0], Sig2max=smax2[0] },

            };
            for (int i = 1; i < nlds; i++)
            {
                records.Add(new SResall()
                {
                    LC = LC[i],
                    Eps1min = emin1[i],
                    Sig1min = smin1[i],
                    Eps2min = emin2[i],
                    Sig2min = smin2[i],
                    Eps1max = emax1[i],
                    Sig1max = smax1[i],
                    Eps2max = emax2[i],
                    Sig2max = smax2[i],
                });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
        }
        public void Converg(Vector<double> Fg, Vector<double> Ff, Vector<double> u, string filename, bool stg2)
        {
            var records = new List<Conv>
            {
                new Conv { Fgiven=Fg[0], Ffound=Ff[0], u=u[0] },

            };
            for (int i = 1; i < Fg.Count(); i++)
            {
                records.Add(new Conv() { Fgiven = Fg[i], Ffound = Ff[i], u = u[i] });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
            if (stg2 == false)
            { Pickle_u(u, "u.csv"); }
        }
        public void Pickle_u(Vector<double> u, string filename)
        {
            var records = new List<U>
            {
                new U { u=u[0] },

            };
            for (int i = 1; i < u.Count(); i++)
            {
                records.Add(new U() { u = u[i] });
            }
            using (var writer = new StreamWriter(Path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                csv.WriteRecords(records);
            }
        }
        public List<Conv> ReadConverg()
        {
            using (var reader = new StreamReader(Path + "Convergence.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<Conv> records = csv.GetRecords<Conv>().ToList();
                return records;
            }
        }
        public List<U> Read_U()
        {
            using (var reader = new StreamReader(Path + "u.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<U> records = csv.GetRecords<U>().ToList();
                return records;
            }
        }
        public List<FResults> ReadFresult()
        {
            using (var reader = new StreamReader(Path + "result.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<FResults> records = csv.GetRecords<FResults>().ToList();
                return records;
            }
        }
        public List<FResall> ReadFresall()
        {
            using (var reader = new StreamReader(Path + "result.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<FResall> records = csv.GetRecords<FResall>().ToList();
                return records;
            }
        }

        public List<SResults> ReadSresult()
        {
            using (var reader = new StreamReader(Path + "result.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<SResults> records = csv.GetRecords<SResults>().ToList();
                return records;
            }
        }
        public List<SResall> ReadSresall()
        {
            using (var reader = new StreamReader(Path + "result.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Dlm;
                List<SResall> records = csv.GetRecords<SResall>().ToList();
                return records;
            }
        }
    }
}