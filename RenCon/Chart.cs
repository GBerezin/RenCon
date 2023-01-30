using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RenCon
{
    public partial class Chart : Form
    {
        public CSV data;
        public string member;
        public Chart(CSV d, string m)
        {
            InitializeComponent();
            data = d;
            member = m;
        }

        private void Chart_Load(object sender, EventArgs e)
        {
            if (member == "Frame")
            {
                chart1.Series["Stress1"].Enabled = false;
                chart1.Series["Stress2"].Enabled = false;
                chart1.DataSource = data.ReadFresult();
                chart1.Series["Stress"].XValueMember = "Zx";
                chart1.Series["Stress"].YValueMembers = "Zy, Stress";
                chart1.Titles[0].Text = "Stress, Mpa";
            }
            else
            {
                chart1.Series["Stress"].Enabled = false;
                chart1.ChartAreas["ChartArea1"].AxisY2.Enabled = AxisEnabled.True;
                chart1.DataSource = data.ReadSresult();
                chart1.Series["Stress1"].XValueMember = "Z";
                chart1.Series["Stress1"].YValueMembers = "Stress1";
                chart1.Series["Stress2"].XValueMember = "Z";
                chart1.Series["Stress2"].YValueMembers = "Stress2";
                chart1.Titles[0].Text = "Stresses, Mpa";
                chart1.Legends.Add("Stress1");
                chart1.Legends.Add("Stress2");
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
