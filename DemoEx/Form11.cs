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
    public partial class Form11 : Form
    {
        object[] status;
        string name_status;
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bs = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public Form11()
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
            string commandStr = "SELECT id_order AS 'Код', numId_order AS 'Номер заказа', users.name_user AS 'Имя пользователя', product.name_product AS 'Товар', count_order AS 'Количество', date_order AS 'Дата заказа', time_order AS 'Время заказа', totalPrise_order AS 'Сумма товара (цена*количество)', status.name_status AS 'Статус заказа' FROM orders INNER JOIN product ON product.id_product=orders.id_products INNER JOIN status ON status.id_status=orders.id_status INNER JOIN users ON users.id_user=orders.id_user ORDER BY orders.id_order ASC";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bs.DataSource = table;
            dataGridView1.DataSource = bs;
            conn.Close();
        }

        public void GetStatusUsers()
        {
            string commandStr = "SELECT * FROM status WHERE id_status = 2 OR id_status = 6";
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM orders";
                conn.Open();
                using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
                {
                    MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                        dt.Rows[dataGridView1.SelectedCells[0].RowIndex][8] = comboBox1.SelectedItem;
                    
                    da.Update(dt);
                }


                conn.Close();
                MessageBox.Show("Статус успешно изменен!");
                reload_list();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            try
            {

                //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
                string connStr = "server=localhost;port=3306;user=root;database=kurs_5;password=root;";
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
    }
}
