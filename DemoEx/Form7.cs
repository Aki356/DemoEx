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
        string name_status;
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
            string commandStr = "SELECT id_user AS 'Код', log_user AS 'Логин', pass_user AS 'Пароль', name_user AS 'Имя', phone_user AS 'Номер тел.', role.name_role AS 'Должность', statususer.name_statusUser AS 'Статус сотрудника' FROM users INNER JOIN statususer ON statususer.id_statusUser=users.statusUser_user INNER JOIN role ON role.id_role=users.id_role WHERE users.id_role NOT IN (SELECT id_role FROM users  WHERE id_role = 0) ORDER BY users.id_user ASC";
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
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            try
            {

                //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
                string connStr = "server=localhost;port=3306;user=root;database=kurs_5;password=root;";
                conn = new MySqlConnection(connStr);
                dataGridView1.AllowUserToAddRows = false;

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                textBox5.ReadOnly = true;

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form3 example = new Form3();
            this.Hide();
            example.ShowDialog();
            this.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM users WHERE users.id_role NOT IN (SELECT id_role FROM users  WHERE id_role = 0)";
            conn.Open();
            using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Rows[dataGridView1.CurrentCell.RowIndex][7] = comboBox1.SelectedItem;

                MessageBox.Show(comboBox1.SelectedItem.ToString());
                da.Update(dt);
            }

            
            conn.Close();
            MessageBox.Show("Регистрация прошла успешно!");
            reload_list();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "1")
            {
                name_status = "Действующий";
            }
            else if (comboBox1.SelectedItem.ToString() == "2")
            {
                name_status = "Уволен";
            }
            else if (comboBox1.SelectedItem.ToString() == "3")
            {
                name_status = "Временно действующий";
            }

            textBox5.Text = name_status.ToString();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}
