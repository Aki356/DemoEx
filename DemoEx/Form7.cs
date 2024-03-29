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
    public partial class Form7 : Form
    {
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bs = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public Form7()
        {
            InitializeComponent();
        }

        public void reload_list()
        {
            table.Clear();
            GetListUsers();
        }
        public void GetListUsers()
        {
            string commandStr = "SELECT id_user AS 'Код', log_user AS 'Логин', pass_user AS 'Пароль', name_user AS 'Имя', phone_user AS 'Номер тел.', id_role AS 'Должность', statusUser_user AS 'Статус сотрудника' FROM users WHERE id_role NOT IN (SELECT id_role FROM users WHERE id_role = 0)";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bs.DataSource = table;
            dataGridView1.DataSource = bs;
            conn.Close();
        }
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
        private void Status()
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if(cell.ColumnIndex == 6)
                    {
                        MessageBox.Show(Convert.ToString(cell.Value));
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Status();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            try
            {

                //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
                string connStr = "server=localhost;port=3306;user=root;database=kurs_5;password=root;";
                conn = new MySqlConnection(connStr);
                GetListUsers();
                dataGridView1.AllowUserToAddRows = false;

                //dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;

                //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                
            }
            catch (Exception ex)
            {
                label6.Text = $"Подключение отсутствует! Возникло исключение: { ex.Message}";
                label6.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
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
                    reload_list();
                }
            }
            catch (Exception ex)
            {
                label6.Text = $"Возникло исключение: { ex.Message}";
                label6.Visible = true;
                reload_list();
            }
        }
    }
}
