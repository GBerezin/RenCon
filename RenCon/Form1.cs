using System;
using System.Windows.Forms;

namespace RenCon
{
    public partial class Form1 : Form
    {
        public string Lim_st, member, mode;
        public double gb1, gb3, v;
        public int fi;
        public CSV data = new CSV();
        public bool stg2 { get; set; }
        FCalc f;
        SCalc s;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox2.SelectedIndex = 0;
            toolStripComboBox3.SelectedIndex = 0;
            gb1 = 1.0;
            member = toolStripComboBox1.Text;
            gb3 = double.Parse(toolStripComboBox2.Text);
            v = double.Parse(toolStripComboBox3.Text);
            Fload();
            label2.Text = dataGridView1.SelectedRows[0].Index.ToString();
            toolStripComboBox4.Items.Add("L/C:selected");
            toolStripComboBox4.Items.Add("L/C:divided");
            toolStripComboBox4.Items.Add("L/C:all");
            toolStripComboBox4.SelectedIndex = 0;
            mode = toolStripComboBox4.Text;
            fi = dataGridView1.SelectedRows[0].Index;
            convergenceToolStripMenuItem.Visible = false;
            resultToolStripMenuItem.Visible = false;
            chartToolStripMenuItem.Visible = false;
            toolTip1.SetToolTip(checkBox1, "You can revise data file now");
            checkBox1.Visible = false;
        }
        public void Fload()
        {
            string filename = "floads.csv";
            dataGridView1.DataSource = data.ReadFloads(filename);
            dataGridView1.CurrentRow.Selected = true;
            ViewFload();
        }
        public void ViewFload()
        {
            vToolStripMenuItem.Visible = false;
            if (radioButton1.Checked == true)
            {
                dataGridView1.Columns[0].Visible = true;
                dataGridView1.Columns[1].Visible = true;
                dataGridView1.Columns[2].Visible = true;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
            }
            else if (radioButton1.Checked == false && radioButton3.Checked == true)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = true;
                dataGridView1.Columns[4].Visible = true;
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
            }
            else
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = true;
                dataGridView1.Columns[7].Visible = true;
                dataGridView1.Columns[8].Visible = true;
            }
        }
        public void Sload()
        {
            string filename = "sloads.csv";
            dataGridView1.DataSource = data.ReadSloads(filename);
            dataGridView1.CurrentRow.Selected = true;
            ViewSload();
        }
        public void ViewSload()
        {
            vToolStripMenuItem.Visible = true;
            if (radioButton1.Checked == true)
            {
                dataGridView1.Columns[0].Visible = true;
                dataGridView1.Columns[1].Visible = true;
                dataGridView1.Columns[2].Visible = true;
                dataGridView1.Columns[3].Visible = true;
                dataGridView1.Columns[4].Visible = true;
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[15].Visible = false;
                dataGridView1.Columns[16].Visible = false;
                dataGridView1.Columns[17].Visible = false;
            }
            else if (radioButton1.Checked == false && radioButton3.Checked == true)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = true;
                dataGridView1.Columns[7].Visible = true;
                dataGridView1.Columns[8].Visible = true;
                dataGridView1.Columns[9].Visible = true;
                dataGridView1.Columns[10].Visible = true;
                dataGridView1.Columns[11].Visible = true;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[15].Visible = false;
                dataGridView1.Columns[16].Visible = false;
                dataGridView1.Columns[17].Visible = false;
            }
            else
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = true;
                dataGridView1.Columns[13].Visible = true;
                dataGridView1.Columns[14].Visible = true;
                dataGridView1.Columns[15].Visible = true;
                dataGridView1.Columns[16].Visible = true;
                dataGridView1.Columns[17].Visible = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            checkBox1.Visible = true;
            convergenceToolStripMenuItem.Visible = true;
            resultToolStripMenuItem.Visible = true;
            chartToolStripMenuItem.Visible = true;
            if (radioButton1.Checked == true)
            {
                Lim_st = "First";
            }
            else
                Lim_st = "Second";
            member = toolStripComboBox1.Text;
            if (member == "Frame")
            {
                f = new FCalc(data, Lim_st, gb1, gb3, stg2);
                StartCalc(f, fi, mode);
                if (mode == "L/C:selected")
                {
                    toolStripStatusLabel1.Text = "Number of iterations: " + f.itrn.ToString() + f.Mes1 + f.Mes2;
                }
                else
                {
                    toolStripStatusLabel1.Text = "LC_min=" + f.lcmin.ToString() + ", LC_max=" + f.lcmax.ToString();
                }
            }
            else
            {
                s = new SCalc(data, Lim_st, gb1, gb3, v, stg2);
                StartCalc(s, fi, mode);
                if (mode == "L/C:selected")
                {
                    toolStripStatusLabel1.Text = "Number of iterations: " + s.itrn.ToString() + s.Mes1 + s.Mes2;
                }
                else
                {
                    toolStripStatusLabel1.Text = "LC_min1=" + s.lcmin1.ToString() + ", LC_min2=" + s.lcmin2.ToString() +
                    ", LC_max1=" + s.lcmax1.ToString() + ", LC_max2=" + s.lcmax2.ToString();
                }
            }
            checkBox1.Checked = false;
            Modevis(mode);
        }
        static void StartCalc(Interface1 calcmode, int fi, string m)
        {
            calcmode.Start(fi, m);
        }
        public void Modevis(string m)
        {
            if (m == "L/C:selected")
            {
                convergenceToolStripMenuItem.Visible = true;
                resultToolStripMenuItem.Visible = true;
                chartToolStripMenuItem.Visible = true;
            }
            else
            {
                convergenceToolStripMenuItem.Visible = false;
                resultToolStripMenuItem.Visible = true;
                chartToolStripMenuItem.Visible = false;
                checkBox1.Checked = false;
                checkBox1.Visible = false;
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            { Lim_st = radioButton.Text; }
            if (member == "Frame")
            { ViewFload(); }
            else
            { ViewSload(); }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            { Lim_st = radioButton.Text; }
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            AboutBox1 abbox = new AboutBox1();
            abbox.Show();
        }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            label2.Text = e.RowIndex.ToString();
            fi = e.RowIndex;
        }
        private void button2_Click_1(object sender, EventArgs e)
        { ActiveForm.Close(); }

        private void toolStripComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            convergenceToolStripMenuItem.Visible = false;
            resultToolStripMenuItem.Visible = false;
            chartToolStripMenuItem.Visible = false;
            member = toolStripComboBox1.Text;
            mode = toolStripComboBox4.Text;
            if (member == "Frame")
            { Fload(); }
            else
            { Sload(); }
        }

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            v = double.Parse(toolStripComboBox3.Text);
        }

        private void convergenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Viewer Viewer1 = new Viewer(data, "Convergence", mode);
            Viewer1.Show();
        }

        private void resultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Viewer Viewer1 = new Viewer(data, member, mode);
            Viewer1.Show();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked == true)
            { gb1 = 1.0; }
            else
            { gb1 = 0.9; }
            if (member == "Frame")
            { ViewFload(); }
            else
            { ViewSload(); }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked == true)
            { gb1 = 0.9; }
            else
            { gb1 = 1.0; }
            if (member == "Frame")
            { ViewFload(); }
            else
            { ViewSload(); }
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            stg2 = checkBox1.Checked;
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index).ToString();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Data dt = new Data(data, member);
            dt.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            string Path = "C:\\data\\";
            Help.ShowHelp(this, Path + "RC.chm");
        }

        private void toolStripComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            member = toolStripComboBox1.Text;
            mode = toolStripComboBox4.Text;
            Modevis(mode);
        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Chart Chart1 = new Chart(data, member);
            Chart1.Show();
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            gb3 = double.Parse(toolStripComboBox2.Text);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        { ActiveForm.Close(); }
    }
}