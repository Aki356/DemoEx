using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DemoEx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 example = new Form3();
            example.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Form1 form1 = new Form1();
            Form2 example = new Form2();
            example.ShowDialog();
            //form1.Hide();
        }
    }
}
