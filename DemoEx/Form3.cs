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
    public partial class Form3 : Form
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

        public bool GetUserInfo(string login, string password)
        {
            string select_id = textBox1.Text;
            conn.Open();
            string sql = $"SELECT * FROM users WHERE log_user='{login}' OR pass_user='{sha256(password)}'";
            DataTable tb = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;

            adapter.Fill(tb);
            label6.Text = Convert.ToString(tb.Rows.Count);
            label6.Visible = true;
            conn.Close();

            if (tb.Rows.Count > 0)
            {
                // User is logged in maybe do FormsAuthentication.SetAuthcookie(username);
                return true;
            }
            else
            {
                // Authentication failed
                return false;
            }
        }
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetUserInfo(textBox1.Text, textBox2.Text) == true)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                }
                else
                {
                    string sql = "SELECT log_user, pass_user, name_user, phone_user FROM users";
                    conn.Open();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
                    {
                        MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        // This is important, because Update will work only on rows
                        // present in the DataTable whose RowState is Added, Modified or Deleted
                        dt.Rows.Add(textBox1.Text, sha256(textBox2.Text), textBox3.Text, textBox4.Text);
                        da.Update(dt);
                    }
                    conn.Close();
                    MessageBox.Show("Авторизация успешна!");
                }
            }
            catch (Exception ex)
            {
                label6.Text = $"Возникло исключение: { ex.Message}";
                label6.Visible = true;
            }
            //string sql = $"INSERT INTO users (log_user, pass_user, name_user, phone_user) VALUES('{textBox1.Text}','{sha256(textBox2.Text)}','{textBox3.Text}','{textBox4.Text}')";

            //MySqlCommand command = new MySqlCommand(sql, conn);

            //command.Parameters.Add("@ul", MySqlDbType.VarChar, 25);
            //command.Parameters.Add("@upw", MySqlDbType.VarChar, 25);
            //command.Parameters.Add("@un", MySqlDbType.VarChar, 25);
            //command.Parameters.Add("@uph", MySqlDbType.VarChar, 25);
            //
            //command.Parameters["@ul"].Value = textBox1.Text;
            //command.Parameters["@upw"].Value = sha256(textBox2.Text);
            //command.Parameters["@un"].Value = textBox3.Text;
            //command.Parameters["@uph"].Value = textBox3.Text;
            //
            //adapter.InsertCommand = command;
            //
            //adapter.Fill(table);

            //if (table.Rows.Count > 0)
            //{
            //this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Неверные данные авторизации!");
            //}
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connStr);
            }
            catch (Exception ex)
            {
                label6.Text = $"Подключение отсутствует! Возникло исключение: { ex.Message}";
                label6.Visible = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
