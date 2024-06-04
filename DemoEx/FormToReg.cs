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
    public partial class FormToReg : Form
    {
        object[] status;
        object[] role;
        string name_status;
        string name_role;
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
        public void GetRoleUsers()
        {
            string commandStr = "SELECT * FROM role";
            conn.Open();
            using (MySqlDataAdapter da = new MySqlDataAdapter(commandStr, conn))
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                // This is important, because Update will work only on rows
                // present in the DataTable whose RowState is Added, Modified or Deleted
                foreach (DataGridViewRow row in dataGridView1.Rows) //перебираем все строки в таблице
                {
                    foreach (DataGridViewCell cell in row.Cells) //перебираем все ячейки в каждой строке
                    {
                        if (cell.ColumnIndex == 0) //проверяем какому столбцу принадлежит ячейка (указать индекс вашего столбца)
                        {
                            role = new object[] { Convert.ToInt32(cell.Value) };

                            comboBox2.Items.AddRange(role);
                        }
                    }
                }
            }
            conn.Close();
        }
        public void GetStatusUsers()
        {
            string commandStr = "SELECT * FROM statususer";
            conn.Open();
            using (MySqlDataAdapter da = new MySqlDataAdapter(commandStr, conn))
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                // This is important, because Update will work only on rows
                // present in the DataTable whose RowState is Added, Modified or Deleted
                foreach (DataGridViewRow row in dataGridView1.Rows) //перебираем все строки в таблице
                {
                    foreach (DataGridViewCell cell in row.Cells) //перебираем все ячейки в каждой строке
                    {
                        if (cell.ColumnIndex == 0) //проверяем какому столбцу принадлежит ячейка (указать индекс вашего столбца)
                        {
                            status = new object[] { Convert.ToInt32(cell.Value) };
                            comboBox3.Items.AddRange(status);
                        }
                    }
                }
            }
            conn.Close();
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
        public FormToReg()
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
                    string sql = "SELECT log_user, pass_user, name_user, phone_user, id_role, statusUser_user FROM users";
                    conn.Open();
                    //DataTable table = new DataTable();
                    //MySqlDataAdapter adapter = new MySqlDataAdapter();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
                    {
                        MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        // This is important, because Update will work only on rows
                        // present in the DataTable whose RowState is Added, Modified or Deleted
                        dt.Rows.Add(textBox1.Text, sha256(textBox2.Text), textBox3.Text, textBox4.Text, comboBox2.SelectedItem, comboBox3.SelectedItem);
                        da.Update(dt);
                    }
                    conn.Close();
                    MessageBox.Show("Регистрация прошла успешно!");
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connStr);
                GetStatusUsers();
                GetRoleUsers();
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Подключение отсутствует! Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
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

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedItem.ToString() == "0")
                {
                    name_status = null;
                }
                else if (comboBox3.SelectedItem.ToString() == "1")
                {
                    name_status = "Действующий";
                }
                else if (comboBox3.SelectedItem.ToString() == "2")
                {
                    name_status = "Уволен";
                }
                else if (comboBox3.SelectedItem.ToString() == "3")
                {
                    name_status = "Временно действующий";
                }

                textBox7.Text = name_status.ToString();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedItem.ToString() == "0")
                {
                    name_role = "Неизвестный";
                }
                else if (comboBox2.SelectedItem.ToString() == "1")
                {
                    name_role = "Администратор";
                }
                else if (comboBox2.SelectedItem.ToString() == "2")
                {
                    name_role = "Официант";
                }
                else if (comboBox2.SelectedItem.ToString() == "3")
                {
                    name_role = "Повар";
                }

                textBox6.Text = name_role.ToString();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
