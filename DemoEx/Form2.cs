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
    public partial class Form2 : Form
    {
        //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
        string connStr = "server=localhost;port=3306;user=root;database=kurs_5;password=root;";
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
                Auth.auth_role = Convert.ToInt32(reader[6].ToString());
            }
            reader.Close();
            conn.Close();
        }
        public Form2(Form1 f1)
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
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
                    MessageBox.Show("Авторизация успешна!");
                    if(Auth.auth_role == 1)
                    {
                        Form4 example = new Form4();
                        this.Hide();
                        example.ShowDialog();
                    }
                    else if (Auth.auth_role == 2)
                    {
                        Form6 example = new Form6();
                        this.Hide();
                        example.ShowDialog();
                    }
                    else if (Auth.auth_role == 3)
                    {
                        Form5 example = new Form5();
                        this.Hide();
                        example.ShowDialog();
                    }
                    else if (Auth.auth_role == 0)
                    {
                        Form1 example = new Form1();
                        example.label2.Text = "Вы вошли, но вы не являетесь сотрудником";
                        this.Hide();
                        example.ShowDialog();
                    }
                    
                    
                }
                else
                {
                    MessageBox.Show("Неверные данные авторизации!");
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
            string a = textBox1.Text;
            label1.Text = a;
        }

        private void Form2_Load(object sender, EventArgs e)
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //textBox3.Text = sha256(textBox2.Text);
        }
    }
}
