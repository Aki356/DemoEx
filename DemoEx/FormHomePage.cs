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
    public partial class FormHomePage : Form
    {
        //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
        string connStr = "server=VH310.spaceweb.ru;port=3308;user=lefleurdru;database=lefleurdru;password=Akiko356Amiko;";
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();

        static string sha256(string pass)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(pass));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public void GetUserInfo(string login)
        {
            string select_id = textBox1.Text;
            conn.Open();
            string sql = $"SELECT * FROM users WHERE log_user='{login}'";
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Auth.auth_id = reader[0].ToString();
                Auth.auth_log = reader[1].ToString();
                Auth.auth_name = reader[3].ToString();
                Auth.auth_role = Convert.ToInt32(reader[5].ToString());
            }
            reader.Close();
            conn.Close();
        }
        public FormHomePage()
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
            try
            {
                FormToReg example = new FormToReg();
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                FormHomePage form1 = new FormHomePage();
                FormToAuth example = new FormToAuth(form1);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connStr);
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Подключение отсутствует! Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void button2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1?.Text) || string.IsNullOrEmpty(textBox2?.Text))
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else
                {
                    string sql = "SELECT * FROM users WHERE log_user = '" + textBox1.Text + "' and pass_user = '" + sha256(textBox2.Text) + "'";
                    conn.Open();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlCommand command = new MySqlCommand(sql, conn);

                    command.Parameters.Add("@ul", MySqlDbType.VarChar, 25);
                    command.Parameters.Add("@up", MySqlDbType.VarChar, 25);

                    command.Parameters["@ul"].Value = textBox1.Text;
                    command.Parameters["@up"].Value = sha256(textBox2.Text);

                    adapter.SelectCommand = command;

                    adapter.Fill(table);
                    conn.Close();

                    if (table.Rows.Count > 0)
                    {
                        Auth.auth = true;
                        GetUserInfo(textBox1.Text);
                        if (Auth.auth_role == 1)
                        {
                            MessageBox.Show("Авторизация успешна!");
                            FormAdmin example = new FormAdmin();
                            this.Hide();
                            example.ShowDialog();
                            this.Show();
                        }
                        else if (Auth.auth_role == 2)
                        {
                            MessageBox.Show("Авторизация успешна!");
                            FormWaiter example = new FormWaiter();
                            this.Hide();
                            example.ShowDialog();
                            this.Show();
                        }
                        else if (Auth.auth_role == 3)
                        {
                            MessageBox.Show("Авторизация успешна!");
                            FormCook example = new FormCook();
                            this.Hide();
                            example.ShowDialog();
                            this.Show();
                        }
                        else if (Auth.auth_role == 0)
                        {
                            MessageBox.Show("Вы вошли, но вы не являетесь сотрудником.");
                        }


                    }
                    else
                    {
                        MessageBox.Show("Неверные данные авторизации!");
                    }
                }
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
