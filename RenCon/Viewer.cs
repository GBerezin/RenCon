using System;
using System.Windows.Forms;

namespace RenCon
{
    public partial class Viewer : Form
    {
        public CSV data;
        public string member;
        public string mode;
        public Viewer(CSV d, string m, string mod)
        {
            InitializeComponent();
            data = d;
            member = m;
            mode = mod;
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            if (member == "Frame" && mode == "L/C:selected")
            {
                dataGridView1.DataSource = data.ReadFresult();
                dataGridView1.Columns[0].Visible = true;
            }
            else if (member == "Frame" && (mode == "L/C:divided" || mode == "L/C:all"))
            {
                dataGridView1.DataSource = data.ReadFresall();
                dataGridView1.Columns[0].Visible = false;
            }
            else if (member == "Slab" && mode == "L/C:selected")
            {
                dataGridView1.DataSource = data.ReadSresult();
                dataGridView1.Columns[0].Visible = true;
            }
            else if (member == "Slab" && (mode == "L/C:divided" || mode == "L/C:all"))
            {
                dataGridView1.DataSource = data.ReadSresall();
                dataGridView1.Columns[0].Visible = false;
            }
            else
            { dataGridView1.DataSource = data.ReadConverg(); }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index).ToString();
        }
    }
}
