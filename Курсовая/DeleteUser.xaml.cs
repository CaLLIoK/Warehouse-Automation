using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace StockRoom
{
    /// <summary>
    /// Логика взаимодействия для DeleteUser.xaml
    /// </summary>
    public partial class DeleteUser : Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";

        public DeleteUser()
        {
            InitializeComponent();
            string users = string.Empty;
            string status = string.Empty;
            ObservableCollection<int> codeList = new ObservableCollection<int>();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = @"SELECT UserCode, UserLogin, UserPassword, AdministratorState, UserName, UserSurname FROM Users ORDER BY UserSurname";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader(); // заполняем данными combobox с кодами пользователей и listbox
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    users = dataReader[0].ToString() + " - " + dataReader[1].ToString() + " " + dataReader[2].ToString() + " - " + dataReader[4].ToString() + " " + dataReader[5].ToString();
                    status = dataReader[3].ToString();
                    if (Convert.ToBoolean(status) == true)
                        users += " (Администратор)";
                    else
                        users += " (Пользователь)";
                    usersList.Items.Add(users);
                    codeList.Add(Convert.ToInt32(dataReader[0].ToString()));
                    var newList = from i in codeList orderby i select i;
                    deleteUser.ItemsSource = newList;
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteUser.Text != string.Empty)
            {
                using (SqlConnection deleteRow = new SqlConnection(connectionString))
                using (SqlCommand lastCommnd = deleteRow.CreateCommand()) // удаляем пользователя
                {
                    lastCommnd.CommandText = "DELETE FROM Users WHERE UserCode = @code";

                    lastCommnd.Parameters.AddWithValue("@code", deleteUser.Text);

                    deleteRow.Open();
                    lastCommnd.ExecuteNonQuery();
                    deleteRow.Close();
                }
                MessageBox.Show("Пользователь удален.");
                deleteUser.SelectedIndex = -1; // убираем выбранный код из combobox
                usersList.Items.Clear();
                string users = string.Empty;
                string status = string.Empty;
                ObservableCollection<int> codeList = new ObservableCollection<int>();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query = @"SELECT UserCode, UserLogin, UserPassword, AdministratorState, UserName, UserSurname FROM Users ORDER BY UserSurname";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows) // обновляем данные combobox и listbox
                {
                    while (dataReader.Read())
                    {
                        users = dataReader[0].ToString() + " - " + dataReader[1].ToString() + " " + dataReader[2].ToString() + " - " + dataReader[4].ToString() + " " + dataReader[5].ToString();
                        status = dataReader[3].ToString();
                        if (Convert.ToBoolean(status) == true)
                            users += " (Администратор)";
                        else
                            users += " (Пользователь)";
                        usersList.Items.Add(users);
                        codeList.Add(Convert.ToInt32(dataReader[0].ToString()));
                        var newList = from i in codeList orderby i select i;
                        deleteUser.ItemsSource = newList;
                    }
                }
            }      
            else
            {
                MessageBox.Show("Вы не указали код пользователя, которого собираетесь удалить.");
                return;
            }
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Mainmenu mainmenu = new Mainmenu();
            mainmenu.Show();
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "udalenie_polzovatelya.htm");
        }
    }
}