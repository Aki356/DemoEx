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
    public partial class FormCookOrders : Form
    {
        object[] status;
        string name_status;
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bs = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public FormCookOrders()
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
            string commandStr = "SELECT id_order AS 'Код', numId_order AS 'Номер заказа', users.name_user AS 'Имя пользователя', product.name_product AS 'Товар', count_order AS 'Количество', date_order AS 'Дата заказа', time_order AS 'Время заказа', totalPrise_order AS 'Сумма товара (цена*количество)', status.name_status AS 'Статус заказа' FROM orders INNER JOIN product ON product.id_product=orders.id_products INNER JOIN status ON status.id_status=orders.id_status INNER JOIN users ON users.id_user=orders.id_user ORDER BY orders.id_order DESC";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bs.DataSource = table;
            dataGridView1.DataSource = bs;
            conn.Close();
        }

        public void GetStatusUsers()
        {
            string commandStr = "SELECT * FROM status WHERE id_status = 3 OR id_status = 4";
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

        private void Form10_Load(object sender, EventArgs e)
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
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[8].ReadOnly = true;
                textBox1.ReadOnly = true;

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


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
            try
            {
                if (string.IsNullOrEmpty(comboBox1?.SelectedItem?.ToString()))
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else
                {
                    string sql = "SELECT * FROM orders ORDER BY orders.id_order DESC";
                    conn.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
                    {
                        MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        using (MySqlCommand command = new MySqlCommand("UPDATE `orders` SET `id_status` = '" + comboBox1.SelectedItem + "' WHERE id_order = '" + dt.Rows[dataGridView1.CurrentCell.RowIndex][0] + "'", conn))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Статус успешно изменен!");
                            da.Update(dt);
                        }
                    }


                    conn.Close();
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
                if (comboBox1.SelectedItem.ToString() == "1")
                {
                    name_status = "В обработке";
                }
                else if (comboBox1.SelectedItem.ToString() == "2")
                {
                    name_status = "Принят";
                }
                else if (comboBox1.SelectedItem.ToString() == "3")
                {
                    name_status = "Готовится";
                }
                else if (comboBox1.SelectedItem.ToString() == "4")
                {
                    name_status = "Готов";
                }
                else if (comboBox1.SelectedItem.ToString() == "5")
                {
                    name_status = "Выдан";
                }
                else if (comboBox1.SelectedItem.ToString() == "6")
                {
                    name_status = "Оплачен";
                }
                else if (comboBox1.SelectedItem.ToString() == "7")
                {
                    name_status = "Отменен";
                }

                textBox1.Text = name_status.ToString();
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

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
