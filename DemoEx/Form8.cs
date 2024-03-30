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
    public partial class Form8 : Form
    {
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bs = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public Form8()
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
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form8_Load(object sender, EventArgs e)
        {
            //try
            //{

                //string connStr = "server=127.0.0.1;port=3306;user=root;database=kurs;";
                string connStr = "server=localhost;port=3306;user=root;database=kurs_5;password=root;";
                conn = new MySqlConnection(connStr);
            GetListUsers();
                dataGridView1.AllowUserToAddRows = false;

                //dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[7].ReadOnly = true;

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            //}
            //catch (Exception ex)
            //{
            //    listBox1.Items.Add($"Возникло исключение: { ex.Message}");
            //    listBox1.HorizontalScrollbar = true;
            //    listBox1.Visible = true;
            //}
        }
    }
}
