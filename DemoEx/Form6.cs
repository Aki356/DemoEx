using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoEx
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Form11 example = new Form11();
                this.Hide();
                example.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }
    }
}
