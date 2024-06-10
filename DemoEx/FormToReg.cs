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
        //переменные для сохранения данных о статусе, должности.
        object[] status;
        object[] role;
        string name_status;
        string name_role;
        //переменная данных для соединения с БД
        string connStr = "server=VH310.spaceweb.ru;port=3308;user=lefleurdru;database=lefleurdru;password=Akiko356Amiko;";
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();

        //метод хэширует пароль в кодировке SHA-256
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

        //метод получает данные о должностях в организации, затем присваивает идентификаторы полю с выпадающим списком
        public void GetRoleUsers()
        {
            string commandStr = "SELECT * FROM role"; //запрос для БД
            conn.Open();
            using (MySqlDataAdapter da = new MySqlDataAdapter(commandStr, conn)) //передача запроса в БД
            {
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                BindingSource bs = new BindingSource();
                da.Fill(dt); //заполнение таблицы данными по запросу
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                foreach (DataGridViewRow row in dataGridView1.Rows) //перебираем все строки в таблице
                {
                    foreach (DataGridViewCell cell in row.Cells) //перебираем все ячейки в каждой строке
                    {
                        if (cell.ColumnIndex == 0) //проверяем какому столбцу принадлежит ячейка (указать индекс вашего столбца)
                        {
                            role = new object[] { Convert.ToInt32(cell.Value) }; //присвоение идентификаторов в массив

                            comboBox2.Items.AddRange(role); //присвоение ячейкам выпадающего списка идентификаторов должностей 
                        }
                    }
                }
            }
            conn.Close();
        }

        //метод получает все данные о статусах пользователей и присваивает идентификаторы статусов выпадающему списку
        public void GetStatusUsers()
        {
            string commandStr = "SELECT * FROM statususer"; //строка запроса для БД
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
                            comboBox3.Items.AddRange(status); //присвоение ячейкам выпадающего списка идентификаторов статусов 
                        }
                    }
                }
            }
            conn.Close();
        }

        //метод проверяющий, есть ли в БД пользователь с такими же логином и паролем
        public bool GetUserInfo(string login, string password)
        {
            conn.Open();
            string sql = $"SELECT * FROM users WHERE log_user='{login}' OR pass_user='{sha256(password)}'"; //запрос для БД
            DataTable tb = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, conn); //исполнение запроса
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command; //присвоение адаптеру результатов запроса

            adapter.Fill(tb); //заполнение таблицы данными, для проверки на существование пользователя с такими же данными
            conn.Close();

            if (tb.Rows.Count > 0)
            {
                return true; //если есть такой пользователь
            }
            else
            {
                return false; //если нет такого пользователя
            }
        }
        public FormToReg()
        {
            InitializeComponent();
        }

        //при нажатии кнопки Зарегистрировать
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1?.Text) || string.IsNullOrEmpty(textBox2?.Text) || string.IsNullOrEmpty(textBox3?.Text) || string.IsNullOrEmpty(comboBox2?.SelectedItem?.ToString()) || string.IsNullOrEmpty(textBox4?.Text) || string.IsNullOrEmpty(comboBox3?.SelectedItem?.ToString()))
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else
                {
                    if (GetUserInfo(textBox1.Text, textBox2.Text) == true)
                    {
                        MessageBox.Show("Такой пользователь уже существует!");
                    }
                    else //если такой пользователь не найден
                    {
                        //запрос для БД
                        string sql = "SELECT log_user, pass_user, name_user, phone_user, id_role, statusUser_user FROM users";
                        conn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn)) //выполнение запроса
                        {
                            MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                            DataTable dt = new DataTable();
                            da.Fill(dt); //заполнение таблицы по запросу
                                         //добавление в таблицу данных пользователя
                            dt.Rows.Add(textBox1.Text, sha256(textBox2.Text), textBox3.Text, textBox4.Text, comboBox2.SelectedItem, comboBox3.SelectedItem);
                            da.Update(dt); //обновление данных
                        }
                        conn.Close();
                        MessageBox.Show("Регистрация прошла успешно!");
                    }
                }
            }
            catch (Exception ex) //ниже блок кода необходим для вывода возникших ошибок
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}"); //вывод ошибки
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connStr); //подключение к БД при загрузке окна
                GetStatusUsers(); //получение и заполнение списков данными статусов
                GetRoleUsers(); //получение и заполнение списков данными должностей
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
            }
            catch (Exception ex) //ниже блок кода необходим для вывода возникших ошибок
            {
                listBox1.Items.Add($"Подключение отсутствует! Возникло исключение: { ex.Message}"); //вывод ошибки
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

        //при выборе в выпадающем списке идентификатора в поле выводится значение статуса(название)
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e) 
        {
            try
            {
                if (comboBox3.SelectedItem.ToString() == "0") //если выбран 0
                {
                    name_status = null;
                }
                else if (comboBox3.SelectedItem.ToString() == "1") //если выбран 1
                {
                    name_status = "Действующий";
                }
                else if (comboBox3.SelectedItem.ToString() == "2") //если выбран 2
                {
                    name_status = "Уволен";
                }
                else if (comboBox3.SelectedItem.ToString() == "3") //если выбран 3
                {
                    name_status = "Временно действующий";
                }

                textBox7.Text = name_status.ToString(); //присвоение данных полю
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"Возникло исключение: { ex.Message}"); //вывод ошибок
                listBox1.HorizontalScrollbar = true;
                listBox1.Visible = true;
            }
        }

        //при выборе в выпадающем списке идентификатора в поле выводится значение должности(название)
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
