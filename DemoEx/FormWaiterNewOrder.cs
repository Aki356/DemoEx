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
        int value;
        int sumAll;
        //переменная данных для соединения с БД
        string connStr = "server=VH310.spaceweb.ru;port=3308;user=lefleurdru;database=lefleurdru;password=Akiko356Amiko;";
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        public FormWaiterNewOrder()
        {
            InitializeComponent();
        }
        //метод для создлания рандомного числа - номера заказа
        public void RandomNum()
        {
            //Создание объекта для генерации чисел
            Random rnd = new Random();

            //Получить случайное число (в диапазоне от 1 до 214748)
            value = rnd.Next(1, 214748);
            
            string commandStr = "SELECT * FROM orders WHERE numId_order = '"+ value + "'"; //строка запроса для БД
            using (MySqlDataAdapter da = new MySqlDataAdapter(commandStr, conn)) //отправление запроса в БД
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                bs.DataSource = dt;
                dataGridView2.DataSource = bs;
                if(dataGridView2.Rows.Count == 0)
                {
                    textBox1.Text = value.ToString();
                }
                else
                {
                    value = rnd.Next(1, 214748);
                    textBox1.Text = value.ToString();
                }

            }
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
                dataGridView2.DataSource = bs;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    product = new object[] { dt.Rows[i][1].ToString() }; //присвоение названий в массив
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
                dataGridView2.DataSource = bs;
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
            try
            {
                conn = new MySqlConnection(connStr); //подключение к БД при загрузке окна
                GetStatusUsers(); //получение и заполнение списков данными статусов
                GetProducts(); //получение и заполнение списков данными продуктов
                RandomNum(); //получение и заполнение первого поля
                textBox6.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox5.ReadOnly = true;
                dateTimePicker1.Format = DateTimePickerFormat.Short;
                dateTimePicker2.Format = DateTimePickerFormat.Time;

                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                // Программное добавление колонок
                dataGridView1.Columns.Add("Column1", "Колонка 1");
                dataGridView1.Columns.Add("Column2", "Колонка 1");
                dataGridView1.Columns.Add("Column3", "Колонка 1");
                dataGridView1.Columns.Add("Column4", "Колонка 1");
                dataGridView1.Columns[0].HeaderText = "Код товара";
                dataGridView1.Columns[1].HeaderText = "Название товара";
                dataGridView1.Columns[2].HeaderText = "Цена товара";
                dataGridView1.Columns[3].HeaderText = "Количество товара";

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                TimeZoneInfo moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
                DateTime moscowTime = DateTime.UtcNow + moscowTimeZone.BaseUtcOffset;
                dateTimePicker2.Value = moscowTime;

                dataGridView1.AllowUserToAddRows = false;
                dataGridView2.AllowUserToAddRows = false;
                textBox2.Text = Auth.auth_name;
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
                    dataGridView2.DataSource = bs;
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
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sql_str = "SELECT id_product, name_product, price_product FROM product WHERE name_product = '" + comboBox1.SelectedItem + "'"; //строка запроса для БД

                conn.Open();
                using (MySqlDataAdapter da = new MySqlDataAdapter(sql_str, conn)) //отправление запроса в БД
                {
                    MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    BindingSource bs = new BindingSource();
                    da.Fill(dt);
                    bs.DataSource = dt;
                    dataGridView2.DataSource = bs;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (comboBox1.SelectedItem != null)
                        {
                            if (comboBox1.SelectedItem.ToString() == dt.Rows[i][1].ToString())
                            {
                                textBox3.Text = dt.Rows[i][0].ToString();
                                textBox5.Text = dt.Rows[i][2].ToString();
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
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(dateTimePicker1.Value.ToString()) || string.IsNullOrEmpty(dateTimePicker2.Value.ToString()) || string.IsNullOrEmpty(textBox7.Text) || string.IsNullOrEmpty(textBox6.Text))
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else
                {
                    if (dataGridView1.Rows.Count != 0)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            sumAll = sumAll + (Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value) * Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value));
                        }
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            int totalPrice = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value) * Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                            string sql = "SELECT numId_order, id_user, id_products, count_order, date_order, time_order, totalPrise_order, payment_order, id_status, sumAll_order FROM orders";
                            conn.Open();

                            using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
                            {
                                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                dt.Rows.Add(Convert.ToInt32(textBox1.Text), Auth.auth_id, Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value), dateTimePicker1.Value.ToString("dd.MM.yyyy"), dateTimePicker2.Value.ToString("HH:mm"), totalPrice, textBox7.Text, Convert.ToInt32(textBox6.Text), sumAll);
                                da.Update(dt);
                            }
                            conn.Close();
                            MessageBox.Show("Заказ добавлен!");
                        }
                        sumAll = 0;
                    }
                    else
                    {
                        MessageBox.Show("Окно заказа пустое! Добавьте товар.");
                    }
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
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

        public bool GetDataGrid()
        {
            bool isset = true;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() != textBox3.Text)
                {
                    isset = true;
                }
                else
                {
                    isset = false;
                    if (isset == false)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value) + Convert.ToInt32(textBox4.Text);
                    }
                    break;
                    
                }
                
            }
            return isset;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox3?.Text) || string.IsNullOrEmpty(textBox4?.Text) || string.IsNullOrEmpty(textBox5?.Text) || string.IsNullOrEmpty(comboBox1?.SelectedItem?.ToString()))
                {
                    MessageBox.Show("Заполните все поля, которые относятся к товару.");
                }
                else
                {
                    if (dataGridView1.Rows.Count != 0)
                    {
                        bool res = GetDataGrid();
                        if (res == true)
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView1);  // this line was missing

                            row.Cells[0].Value = Convert.ToInt32(textBox3.Text);
                            row.Cells[1].Value = comboBox1.SelectedItem.ToString();
                            row.Cells[2].Value = Convert.ToInt32(textBox5.Text);
                            row.Cells[3].Value = Convert.ToInt32(textBox4.Text);
                            dataGridView1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridView1);  // this line was missing
                        row.Cells[0].Value = Convert.ToInt32(textBox3.Text);
                        row.Cells[1].Value = comboBox1.SelectedItem.ToString();
                        row.Cells[2].Value = Convert.ToInt32(textBox5.Text);
                        row.Cells[3].Value = Convert.ToInt32(textBox4.Text);
                        dataGridView1.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count != 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить позицию позицию в заказе?", "Подтверждение удаления позиции в заказе", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) //если ответили да, то произойдёт удаление пользователя
                    {
                        try
                        {
                            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                            sumAll -= Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value);
                        }
                        catch (Exception ex)
                        {
                            listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                            listBox1.HorizontalScrollbar = true;
                            listBox1.Visible = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Окно заказа пустое! Добавьте товар.");
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}");
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                RandomNum();
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
