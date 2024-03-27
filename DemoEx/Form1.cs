﻿using System;
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
        public void ChangeTextInTextBox(string newText)
        {
            label2.Text = newText;
            button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 example = new Form3();
            this.Hide();
            example.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            Form2 example = new Form2(form1);
            this.Hide();
            example.ShowDialog();
            this.Show();
            //form1.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
