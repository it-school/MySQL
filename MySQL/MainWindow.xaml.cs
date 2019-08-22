using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

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
            string connStr = "server=192.168.1.152;port=3309;user=itschool;database=itschool;";
            //            string connStr = "server=127.0.0.1;port=3306;user=root;password=password;database=test;";
            // создаём объект для подключения к БД
            conn = new MySqlConnection(connStr);
            RefreshAll();
        }

        public void RefreshAll()
        {
            gridView.ItemsSource = Select().DefaultView;

            cmbUsers.ItemsSource = Select().DefaultView;
            cmbUsers.DisplayMemberPath = "login";
            cmbUsers.SelectedValuePath = "id";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowDataInRichTextBox();
        }

        public void Update(string query = "")
        {
            if (query.Equals(""))
            {
                Random r = new Random();
                int n = 10 + r.Next(100);
                query = $"INSERT INTO users VALUES (NULL, 'user{n}', 'user{n}', 'user{n}', NOW())";
            }

            // устанавливаем соединение с БД
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //            MessageBox.Show(query);
            MySqlCommand MyCommand = new MySqlCommand(query, conn);
            try
            {
                MyCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            RefreshAll();

            conn.Close();
        }

        public DataTable Select(string query = "SELECT id, name, login, password, DATE_FORMAT(regdate, '%d.%m.%Y  %T') AS regdate FROM users")
        {
            // устанавливаем соединение с БД
            if (conn.State != ConnectionState.Open)
                conn.Open();

            // объект для выполнения SQL-запроса
            MySqlCommand sqlCom = new MySqlCommand(query, conn);

            // выполняем запрос и получаем ответ
            //            string name = sqlCom.ExecuteScalar().ToString();
            //            MessageBox.Show(name);

            // выполняем запрос и получаем ответ
            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            return dt;
        }

        public void ShowDataInRichTextBox()
        {
            // устанавливаем соединение с БД
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string query = "SELECT * FROM users";
            richTextBox1.Items.Clear();

            richTextBox1.Items.Add("1 вариант\n");
            var myData = Select(query).Select();
            for (int i = 0; i < myData.Length; i++)
            {
                for (int j = 0; j < myData[i].ItemArray.Length; j++)
                    richTextBox1.Items.Add(myData[i].ItemArray[j] + " ");
                richTextBox1.Items.Add("\n");
            }


            richTextBox1.Items.Add("-----------------\n 2 вариант\n");


            MySqlDataReader MyDataReader = null;
            MySqlCommand sqlCom = null;

            try
            {
                sqlCom = new MySqlCommand(query, conn);
                MyDataReader = sqlCom.ExecuteReader();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (MyDataReader != null && MyDataReader.HasRows)
                while (MyDataReader.Read())
                {
                    int id = MyDataReader.GetInt32(0); //Получаем целое число
                    string name1 = MyDataReader.GetString(1); //Получаем строку

                    richTextBox1.Items.Add(id + "\t" + MyDataReader.GetString(1) + "\t" + MyDataReader.GetString(2));
                }

            // закрываем соединение с БД
            conn.Close();
        }

        private void btnUpdateClick(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void GridView_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string query = "";
            DataRowView dataRow = (DataRowView)gridView.SelectedItem;
            MessageBox.Show(gridView.SelectedIndex.ToString());
            if (gridView.SelectedIndex < gridView.Items.Count - 2)
            {
                //                int index = gridView.CurrentCell.Column.DisplayIndex;
                string columnName = gridView.CurrentCell.Column.Header.ToString();
                string newValue = ((TextBox)e.EditingElement).Text;
                int id = (int)dataRow.Row.ItemArray[0];

                query = "UPDATE users SET " + columnName + "=\"" + newValue + "\" WHERE id=" + id;
            }
            else
            {
                for (int i = 1; i < dataRow.Row.Table.Columns.Count; i++)
                    if (gridView.SelectedItems[i].ToString().Equals(""))
                        return;

                query = $"INSERT INTO users VALUES (NULL, {dataRow.Row.ItemArray[1]}, {dataRow.Row.ItemArray[2]}, {dataRow.Row.ItemArray[3]}, NOW());";
                MessageBox.Show(query);
            }
            Update(query);
        }

        private void GridView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            mainWindow.Title = e.Source.ToString();
        }

        private void CmbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridView.ItemsSource = Select("SELECT * FROM users WHERE id = " + ((ComboBox)sender).SelectedValue.ToString()).DefaultView;
        }
    }
}