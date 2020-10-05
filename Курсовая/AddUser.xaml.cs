using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        SqlConnection myConnectionString = new SqlConnection(@"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockRoom; Integrated Security=True");

        public AddUser()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text != CheckUser.CheckUserLogin(login.Text))
            {
                MessageBox.Show(CheckUser.CheckUserLogin(login.Text));
                return;
            }
            string mySelectQuery = "SELECT * FROM Users WHERE [UserLogin] = '" + login.Text + "'";
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(mySelectQuery, myConnectionString)) // проверяем, занят ли логин
            {
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MessageBox.Show("Этот логин занят. Введите другой");
                    return;
                }
            }

            if (password.Password.ToString() != CheckUser.CheckUserPassword(password.Password.ToString()))
            {
                MessageBox.Show(CheckUser.CheckUserPassword(password.Password.ToString()));
                return;
            }

            if (name.Text != CheckUser.CheckUserName(name.Text))
            {
                MessageBox.Show(CheckUser.CheckUserName(name.Text));
                return;
            }

            if (surname.Text != CheckUser.CheckUserSurname(surname.Text))
            {
                MessageBox.Show(CheckUser.CheckUserSurname(surname.Text));
                return;
            }

            string connectionString = @"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockRoom; Integrated Security=True";

            using (SqlConnection addRow = new SqlConnection(connectionString))
            using (SqlCommand lastCommnd = addRow.CreateCommand()) //здесь добавляем пользователя в систему
            {
                lastCommnd.CommandText = "INSERT INTO Users (UserLogin, UserPassword, UserName, UserSurname) VALUES (@login, @password, @name, @surname)";

                lastCommnd.Parameters.AddWithValue("@login", login.Text);
                lastCommnd.Parameters.AddWithValue("@password", password.Password.ToString());
                lastCommnd.Parameters.AddWithValue("@surname", surname.Text);
                lastCommnd.Parameters.AddWithValue("@name", name.Text);
                addRow.Open();
                lastCommnd.ExecuteNonQuery();
                addRow.Close();
            }
            MessageBox.Show("Пользователь добавлен.");
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
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "dobavlenie_polzovatelya.htm");
        }
    }
}