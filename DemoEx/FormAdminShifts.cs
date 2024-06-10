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
    public partial class FormAdminShifts : Form
    {
        object[] id;
        object[] role;

        string name_id;
        string name_role;
        int id_role;
        //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
        string connStr = "server=VH310.spaceweb.ru;port=3308;user=lefleurdru;database=lefleurdru;password=Akiko356Amiko;";
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bs = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public FormAdminShifts()
        {
            InitializeComponent();
        }
        public void GetListUsers()
        {
            string commandStr = "SELECT id_shift AS 'Код', date_shift AS 'Дата смены', time_shift AS 'Время смены', users.name_user AS 'Имя сотрудника', role.name_role AS 'Должность' FROM shifts INNER JOIN users ON users.id_user=shifts.id_user INNER JOIN role ON role.id_role=shifts.role_user ORDER BY shifts.id_shift DESC";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bs.DataSource = table;
            dataGridView1.DataSource = bs;
            conn.Close();
        }
        public void reload_list()
        {
            table.Clear();
            GetListUsers();
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

                            //comboBox2.Items.AddRange(role);
                        }
                    }
                }
            }
            conn.Close();
            reload_list();
        }
        public void GetIdUsers()
        {
            string commandStr = "SELECT * FROM users WHERE users.id_role NOT IN (SELECT id_role FROM users  WHERE id_role = 0)";
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
                            id = new object[] { Convert.ToInt32(cell.Value) };
                            comboBox1.Items.AddRange(id);
                        }
                    }
                }
            }
            conn.Close();
            reload_list();
        }

        public void Role(int i, DataTable dt)
        {
            //MessageBox.Show(dt.Rows.Count.ToString());
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (comboBox1.SelectedItem != null)
                {
                    if (comboBox1.SelectedItem.ToString() == dt.Rows[i][0].ToString())
                    {
                        name_id = dt.Rows[i][3].ToString();
                        name_role = dt.Rows[i][6].ToString();
                        id_role = Convert.ToInt32(dt.Rows[i][5].ToString());

                        //MessageBox.Show(id_role + " " + name_role + " " + name_id);
                    }
                }
            }

        }

        public void SetectedCB2()
        {
            string commandStr = "SELECT id_user AS 'Код', log_user AS 'Логин', pass_user AS 'Пароль', name_user AS 'Имя', phone_user AS 'Номер тел.', users.id_role AS 'Код должности', role.name_role AS 'Должность', statususer.name_statusUser AS 'Статус сотрудника' FROM users INNER JOIN statususer ON statususer.id_statusUser=users.statusUser_user INNER JOIN role ON role.id_role=users.id_role WHERE users.id_role NOT IN (SELECT id_role FROM users  WHERE id_role = 0) ORDER BY `Код` DESC";
            conn.Open();
            table.Clear();
            using (MySqlDataAdapter da = new MySqlDataAdapter(commandStr, conn))
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                int key = 0;
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;

                Roles roles = new Roles(Role);
                roles(key, dt);
            }
            conn.Close();
            reload_list();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(dateTimePicker1?.Value.ToString()) || string.IsNullOrEmpty(dateTimePicker2?.Value.ToString()) || string.IsNullOrEmpty(dateTimePicker3?.Value.ToString()) || string.IsNullOrEmpty(comboBox1?.SelectedItem?.ToString()) || string.IsNullOrEmpty(textBox3?.Text))
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else
                {
                    string sql = "SELECT date_shift, time_shift, id_user, role_user FROM shifts";
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
                        dt.Rows.Add(dateTimePicker1.Value.ToString("dd.MM.yyyy"), "" + dateTimePicker2.Value.ToString("HH:mm") + " - " + dateTimePicker3.Value.ToString("HH:mm") + "", comboBox1.SelectedItem, Convert.ToInt32(textBox3.Text));
                        da.Update(dt);
                    }
                    conn.Close();
                    MessageBox.Show("Смена добавлена!");
                    reload_list();
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetectedCB2();
                textBox1.Text = name_id.ToString();
                textBox2.Text = name_role.ToString();
                textBox3.Text = id_role.ToString();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connStr);
                GetIdUsers();
                GetRoleUsers();
                dataGridView1.AllowUserToAddRows = false;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;

                dateTimePicker1.Format = DateTimePickerFormat.Short;
                dateTimePicker2.Format = DateTimePickerFormat.Time;
                dateTimePicker3.Format = DateTimePickerFormat.Time;
                dateTimePicker2.ShowUpDown = true;
                dateTimePicker3.ShowUpDown = true;

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            
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

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
        
        public delegate void Roles(int key, DataTable dt);

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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
    }
}
