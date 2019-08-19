using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace MySQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MySqlConnection conn;
        public MainWindow()
        {
            InitializeComponent();

            // строка подключения к БД
            // string connStr = "server=https://remotemysql.com;port=3306;user=Bty2hZRByP;database=Bty2hZRByP;password=YvfJSreBZB";
            string connStr = "server=127.0.0.1;port=3306;user=root;password=password;database=test;";
            // создаём объект для подключения к БД
            conn = new MySqlConnection(connStr);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Select();            

        }



        public void Update()
        {
            // устанавливаем соединение с БД
            if (conn.State != ConnectionState.Open)
                conn.Open();


            MySqlCommand MyCommand = new MySqlCommand("INSERT INTO users VALUES (NULL, 'user111', 'user111', 'user111')", conn);
            MyCommand.ExecuteNonQuery();
            conn.Close();
        }

        public void Select()
        {
            // устанавливаем соединение с БД
            conn.Open();

            // Строка запроса
            string sql = "SELECT * FROM users";
            // объект для выполнения SQL-запроса
            MySqlCommand sqlCom = new MySqlCommand(sql, conn);

            // выполняем запрос и получаем ответ
            string name = sqlCom.ExecuteScalar().ToString();
            MessageBox.Show(name);

            // выполняем запрос и получаем ответ
            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            var myData = dt.Select();
            for (int i = 0; i < myData.Length; i++)
            {
                for (int j = 0; j < myData[i].ItemArray.Length; j++)
                    richTextBox1.Items.Add(myData[i].ItemArray[j] + " ");
                richTextBox1.Items.Add("\n");
            }

            MySqlDataReader MyDataReader;
            MyDataReader = sqlCom.ExecuteReader();
            while (MyDataReader.Read())
            {
                int id = MyDataReader.GetInt32(0); //Получаем целое число
                string name1 = MyDataReader.GetString(1); //Получаем строку

                richTextBox1.Items.Add(id + "\t" + MyDataReader.GetString(1) + "\t" + MyDataReader.GetString(2));
            }

            // закрываем соединение с БД
            conn.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Update();
        }
    }
}
