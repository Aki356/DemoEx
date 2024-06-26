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
    public partial class FormAdminEmployees : Form
    {
        object[] status;
        string name_status;
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bs = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public FormAdminEmployees()
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

        public void GetStatusUsers()
        {
            string commandStr = "SELECT * FROM statususer WHERE name_statusUser = 'Уволен'";
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
                            comboBox1.Items.AddRange(status);
                        }
                    }
                }
            }
            conn.Close();
            reload_list();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            try
            {

                //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
                string connStr = "server=VH310.spaceweb.ru;port=3308;user=lefleurdru;database=lefleurdru;password=Akiko356Amiko;";
                conn = new MySqlConnection(connStr);
                GetStatusUsers();
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

                reload_list();
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
            try
            {
                FormToReg example = new FormToReg();
                this.Hide();
                example.ShowDialog();
                this.Show();
                reload_list();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        //при нажатии кнопки Изменить
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if(comboBox1?.SelectedItem is null || Convert.ToInt32(comboBox1?.SelectedItem) == 0)
                {
                    MessageBox.Show("Ошибка! Проверьте введенные данные.");
                }
                else
                {
                    //строка запроса для БД
                    string sql = "SELECT * FROM users WHERE users.id_role NOT IN (SELECT id_role FROM users  WHERE id_role = 0)";
                    conn.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn)) //выполнение запроса
                    {
                        MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                        DataTable dt = new DataTable();
                        da.Fill(dt); //заполнение таблицы данными по запросу
                        dt.Rows[dataGridView1.CurrentCell.RowIndex][6] = comboBox1.SelectedItem; //изменение статуса
                        da.Update(dt); //обновление данных
                    }
                    conn.Close();
                    MessageBox.Show("Статус успешно изменен!");
                    reload_list(); //метод обновляющий таблицу
                }
            }
            catch (Exception ex) //блок кода для вывода ошибок
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
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
                else
                {
                    name_status = null;
                }

                textBox5.Text = name_status;
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

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //при нажатии Удалить
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //запрос для БД
                string sql = "SELECT * FROM users WHERE users.id_role NOT IN (SELECT id_role FROM users  WHERE id_role = 0)";
                conn.Open();
                using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn)) //выполнение запроса
                {
                    MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt); //заполнение таблицы
                    //вывод сообщения о подтверждении действия
                    DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить пользователя " + dt.Rows[dataGridView1.CurrentCell.RowIndex][3] + " под кодом " + dt.Rows[dataGridView1.CurrentCell.RowIndex][0], "Подтверждение удаления пользователя", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) //если ответили да, то произойдёт удаление пользователя
                    {
                        try
                        {
                            //выполнение удаления пользователя по его идентификатору из выбранной строки в таблице
                            using (MySqlCommand command = new MySqlCommand("DELETE FROM users WHERE id_user = '" + dt.Rows[dataGridView1.CurrentCell.RowIndex][0] + "'", conn))
                            {
                                command.ExecuteNonQuery();
                                MessageBox.Show("Пользователь успешно удален!");
                                da.Update(dt);
                            }
                        }
                        catch (Exception ex)
                        {
                            listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                            listBox1.HorizontalScrollbar = true;
                            listBox1.Visible = true;
                        }
                        //reload_list();
                    }
                    conn.Close();
                    reload_list(); //обновление таблицы
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
