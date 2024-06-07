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
        string connStr = "server=VH310.spaceweb.ru;port=3308;user=lefleurdru;database=lefleurdru;password=Akiko356Amiko;";
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        public FormWaiterNewOrder()
        {
            InitializeComponent();
        }
        //метод для создлания рандомного числа - номера заказа
        public int RandomNum()
        {
            //Создание объекта для генерации чисел
            Random rnd = new Random();

            //Получить случайное число (в диапазоне от 1 до 214748)
            int value = rnd.Next(1, 214748);

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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    product = new object[] { dt.Rows[i][1].ToString() }; //присвоение идентификаторов в массив
                    comboBox1.Items.AddRange(product);
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    status = new object[] { dt.Rows[i][1].ToString() }; //присвоение идентификаторов в массив
                    comboBox2.Items.AddRange(status);
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
            GetProducts(); //получение и заполнение списков данными продуктов
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
            string sql_str = "SELECT * FROM status WHERE name_status = '" + comboBox2.SelectedItem + "'"; //строка запроса для БД

            conn.Open();
            using (MySqlDataAdapter da = new MySqlDataAdapter(sql_str, conn)) //отправление запроса в БД
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (comboBox2.SelectedItem != null)
                    {
                        if (comboBox2.SelectedItem.ToString() == dt.Rows[i][1].ToString())
                        {
                            textBox6.Text = dt.Rows[i][0].ToString();
                        }
                        else
                        {
                            listBox1.Items.Add($"Выбранный элемент не совпадает ни с одним из БД");
                            listBox1.HorizontalScrollbar = true;
                            listBox1.Visible = true;
                        }
                    }
                }
            }
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sql_str = "SELECT id_product, name_product FROM product WHERE name_product = '" + comboBox1.SelectedItem + "'"; //строка запроса для БД

                conn.Open();
                using (MySqlDataAdapter da = new MySqlDataAdapter(sql_str, conn)) //отправление запроса в БД
                {
                    MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    BindingSource bs = new BindingSource();
                    da.Fill(dt);
                    bs.DataSource = dt;
                    dataGridView1.DataSource = bs;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (comboBox1.SelectedItem != null)
                        {
                            if (comboBox1.SelectedItem.ToString() == dt.Rows[i][1].ToString())
                            {
                                textBox3.Text = dt.Rows[i][0].ToString();
                            }
                            else
                            {
                                listBox1.Items.Add($"Выбранный элемент не совпадает ни с одним из БД");
                                listBox1.HorizontalScrollbar = true;
                                listBox1.Visible = true;
                            }
                        }
                    }
                }
                conn.Close();
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
