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
    public partial class FormWaiterNewOrder : Form
    {
        //переменные для сохранения данных о статусе, товаре
        object[] status;
        object[] product;
        string name_status;
        string id_product;
        //переменная данных для соединения с БД
        string connStr = "server=localhost;port=3306;user=root;database=kurs_5;password=root;";
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        public FormWaiterNewOrder()
        {
            InitializeComponent();
        }

        //метод получает все данные о товарах и присваивает название товаров выпадающему списку
        public void GetProducts()
        {
            string commandStr = "SELECT * FROM product"; //строка запроса для БД
            conn.Open();
            using (MySqlDataAdapter da = new MySqlDataAdapter(commandStr, conn)) //отправление запроса в БД
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                foreach (DataGridViewRow row in dataGridView1.Rows) //перебираем все строки в таблице
                {
                    foreach (DataGridViewCell cell in row.Cells) //перебираем все ячейки в каждой строке
                    {
                        if (cell.ColumnIndex == 0) //проверяем какому столбцу принадлежит ячейка (указать индекс вашего столбца)
                        {
                            //product = new object[] { cell.Value?.ToString() ?? String.Empty };
                            product = new object[] { Convert.ToInt32(cell.Value) }; //присвоение идентификаторов в массив
                            comboBox1.Items.AddRange(product); //присвоение ячейкам выпадающего списка идентификаторов статусов 
                        }
                    }
                }
            }
            conn.Close();
        }

        //метод получает все данные о статусах заказов и присваивает названия статусов выпадающему списку
        public void GetStatusUsers()
        {
            string commandStr = "SELECT * FROM status"; //строка запроса для БД
            conn.Open();
            using (MySqlDataAdapter da = new MySqlDataAdapter(commandStr, conn)) //отправление запроса в БД
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                foreach (DataGridViewRow row in dataGridView1.Rows) //перебираем все строки в таблице
                {
                    foreach (DataGridViewCell cell in row.Cells) //перебираем все ячейки в каждой строке
                    {
                        if (cell.ColumnIndex == 0) //проверяем какому столбцу принадлежит ячейка (указать индекс вашего столбца)
                        {
                            status = new object[] { Convert.ToInt32(cell.Value) }; //присвоение идентификаторов в массив
                            comboBox2.Items.AddRange(status); //присвоение ячейкам выпадающего списка идентификаторов статусов 
                        }
                    }
                }
            }
            conn.Close();
        }
        private void Form12_Load(object sender, EventArgs e)
        {
            //try
            //{
                conn = new MySqlConnection(connStr); //подключение к БД при загрузке окна
            GetStatusUsers(); //получение и заполнение списков данными статусов
            GetProducts(); //получение и заполнение списков данными статусов
                textBox6.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;

                textBox2.Text = Auth.auth_name;
            //}
            //catch (Exception ex)
            //{
            //    listBox1.Items.Add($"Возникло исключение: { ex.Message}");
            //    listBox1.HorizontalScrollbar = true;
            //    listBox1.Visible = true;
            //}
        }
        string commandStr = "SELECT orders.id_order orders.numId_order, product.name_product, orders.count_order, orders.date_order, orders.time_order, orders.totalPrise_order, status.name_status FROM `orders` JOIN users ON orders.id_user = users.id_user JOIN status ON orders.id_status = status.id_status JOIN product ON orders.id_products = product.id_product ORDER BY id_order DESC";
        

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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
                textBox3.Text = id_product.ToString();
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
            string sql = "SELECT numId_order, id_user, id_products, count_order, date_order, time_order, totalPrice_order, payment_order, id_status FROM orders";
            conn.Open();
            //DataTable table = new DataTable();
            //MySqlDataAdapter adapter = new MySqlDataAdapter();

            using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Rows.Add(Convert.ToInt32(textBox1.Text), Auth.auth_id, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), dateTimePicker1.Value.ToString("dd.MM.yyyy"), dateTimePicker2.Value.ToString("HH:mm"), Convert.ToInt32(textBox5.Text), textBox7.Text, Convert.ToInt32(textBox6.Text));
                da.Update(dt);
            }
            conn.Close();
            MessageBox.Show("Заказ добавлен!");
        }
    }
}
