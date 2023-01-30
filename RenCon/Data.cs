using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RenCon
{
    public partial class Data : Form
    {
        public CSV data;
        public List<Materials> mat;
        public string member;
        public Data(CSV d, string m)
        {
            InitializeComponent();
            data = d;
            member = m;
        }

        private void Data_Load(object sender, EventArgs e)
        {
            mat = data.ReadMat();

            foreach (Materials m in mat)
            {
                if (m.T == "concrete")
                { comboBox1.Items.Add(m.Grade); }
                else
                { comboBox2.Items.Add(m.Grade); }
            }
            comboBox2.SelectedIndex = 2;
            Rebars();
        }
        public void Rebars()
        {
            if (member == "Frame")
            {
                comboBox1.SelectedIndex = 3;
                ((Control)tabPage1).Enabled = true;
                ((Control)tabPage2).Enabled = false;
                tabControl1.SelectedTab = tabPage1;
                DataGridViewTextBoxColumn Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "Zx";
                DataGridViewTextBoxColumn Col2 = new DataGridViewTextBoxColumn();
                Col2.HeaderText = "Zy";
                DataGridViewTextBoxColumn Col3 = new DataGridViewTextBoxColumn();
                Col3.HeaderText = "A";
                dataGridView1.Columns.AddRange(new[] { Col1, Col2, Col3 });
                dataGridView1.Rows.Add("0.21", "-0.16", "0.0006158");
                dataGridView1.Rows.Add("0.21", "0.0", "0.0001539");
                dataGridView1.Rows.Add("0.21", "0.16", "0.0006158");
                dataGridView1.Rows.Add("0.0", "-0.16", "0.0001539");
                dataGridView1.Rows.Add("0.0", "0.16", "0.0001539");
                dataGridView1.Rows.Add("-0.21", "-0.16", "0.0006158");
                dataGridView1.Rows.Add("-0.21", "0.0", "0.0001539");
                dataGridView1.Rows.Add("-0.21", "0.16", "0.0006158");
            }
            else
            {
                comboBox1.SelectedIndex = 9;
                ((Control)tabPage1).Enabled = false;
                ((Control)tabPage2).Enabled = true;
                tabControl1.SelectedTab = tabPage2;
                DataGridViewTextBoxColumn Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "Z";
                DataGridViewTextBoxColumn Col2 = new DataGridViewTextBoxColumn();
                Col2.HeaderText = "A";
                DataGridViewTextBoxColumn Col3 = new DataGridViewTextBoxColumn();
                Col3.HeaderText = "n";
                DataGridViewTextBoxColumn Col4 = new DataGridViewTextBoxColumn();
                Col4.HeaderText = "alpha";
                dataGridView1.Columns.AddRange(new[] { Col1, Col2, Col3, Col4 });
                dataGridView1.Rows.Add("0.178", "0.002454", "5", "0");
                dataGridView1.Rows.Add("0.153", "0.002454", "5", "90");
                dataGridView1.Rows.Add("-0.153", "0.002454", "5", "90");
                dataGridView1.Rows.Add("-0.178", "0.002454", "5", "0");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (member == "Frame")
            { FdataSave(); }
            else
            { SdataSave(); }
            ActiveForm.Close();
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index).ToString();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text != member)
            {
                dataGridView1.Enabled = false;
                dataGridView1.Visible = false;
                button1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }
            else
            {
                dataGridView1.Enabled = true;
                dataGridView1.Visible = true;
                button1.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
        }
        public void FdataSave()
        {
            double w = Convert.ToDouble(textBox3.Text);
            double h = Convert.ToDouble(textBox4.Text);
            double s = Convert.ToDouble(textBox5.Text);
            int nw = (int)(w / s);
            int nh = (int)(h / s);
            int nc1 = 2 * (nw + nh);
            int nc2 = nw * nh;
            int nc = nc1 + nc2;
            int nr = dataGridView1.Rows.Count - 1;
            int n = nc + nr;
            double Ai = w * h / (nw * nh);
            double dw = w / nw;
            double dh = h / nh;
            string[] Gr = new string[n];
            double[] zx = new double[n];
            double[] zy = new double[n];
            double[] a = new double[n];
            for (int i = 0; i < nw + 1; i++)
            {
                Gr[i] = comboBox1.Text;
                Gr[i + nw + 1] = comboBox1.Text;
                zx[i] = -h / 2;
                zx[i + nw + 1] = h / 2;
                zy[i] = -w / 2 + i * dw;
                zy[i + nw + 1] = -w / 2 + i * dw;
            }
            for (int i = 0; i < nh - 1; i++)
            {
                Gr[i + (nw + 1) * 2] = comboBox1.Text;
                Gr[i + nh - 1 + (nw + 1) * 2] = comboBox1.Text;
                zy[i + (nw + 1) * 2] = -w / 2;
                zy[i + nh - 1 + (nw + 1) * 2] = w / 2;
                zx[i + (nw + 1) * 2] = -h / 2 + dh + i * dh;
                zx[i + nh - 1 + (nw + 1) * 2] = -h / 2 + dh + i * dh;
            }
            for (int i = 0; i < nw; i++)
            {
                for (int j = 0; j < nh; j++)
                {
                    Gr[i * nh + j + nc1] = comboBox1.Text;
                    zx[i * nh + j + nc1] = -h / 2 + dh / 2 + j * dh;
                    zy[i * nh + j + nc1] = -w / 2 + dw / 2 + i * dw;
                    a[i * nh + j + nc1] = Ai;
                }
            }
            for (int i = 0; i < nr; i++)
            {
                Gr[nc + i] = comboBox2.Text;
                zx[nc + i] = Convert.ToDouble(dataGridView1[0, i].Value);
                zy[nc + i] = Convert.ToDouble(dataGridView1[1, i].Value);
                a[nc + i] = Convert.ToDouble(dataGridView1[2, i].Value);
            }
            data.Fdata(Gr, zx, zy, a, "fdata.csv");
        }
        public void SdataSave()
        {
            double h = Convert.ToDouble(textBox1.Text);
            int nc = Convert.ToInt32(textBox2.Text);
            int nr = dataGridView1.Rows.Count - 1;
            double t = h / nc;
            int n = nc + nr + 2;
            string[] Gr = new string[n];
            double[] z = new double[n];
            double[] a = new double[n];
            int[] ni = new int[n];
            double[] al = new double[n];
            for (int i = 0; i < (nc + 2); i++)
            {
                Gr[i] = comboBox1.Text;
                z[i] = h / 2 + 1.5 * t - (i + 1) * t;
                a[i] = t;
                ni[i] = 1;
            }
            z[0] = h / 2;
            z[nc + 1] = -h / 2;
            a[0] = 0;
            a[nc + 1] = 0;
            for (int i = 0; i < nr; i++)
            {
                Gr[nc + 2 + i] = comboBox2.Text;
                z[nc + 2 + i] = Convert.ToDouble(dataGridView1[0, i].Value);
                a[nc + 2 + i] = Convert.ToDouble(dataGridView1[1, i].Value);
                ni[nc + 2 + i] = Convert.ToInt32(dataGridView1[2, i].Value);
                al[nc + 2 + i] = Convert.ToDouble(dataGridView1[3, i].Value);
            }
            data.Sdata(Gr, z, a, ni, al, "sdata.csv");
        }
    }
}
