using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// Логика взаимодействия для LoginFormAdmin.xaml
    /// </summary>
    public partial class LoginFormAdmin : Window
    {
        public LoginFormAdmin()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userLogin = loginField.Text;
            string userPassword = passwordField.Password.ToString();

            string myConnectionString = @"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockRoom; Integrated Security=True";
            string mySelectQuery = "SELECT * FROM Users WHERE [UserLogin] = '" + userLogin + "'and [UserPassword]='" + userPassword + "' and [AdministratorState] = 'true'";
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(mySelectQuery, myConnectionString)) // проверка введенных данных
            {
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    FileStream fs = new FileStream("UserLogin.txt", FileMode.Create);
                    StreamWriter loginFile = new StreamWriter(fs);            
                    loginFile.Close();
                    Mainmenu mainmenu = new Mainmenu();
                    mainmenu.Show();
                    this.Close();
                }
                else if (table.Rows.Count == 0)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "nachalnoe_okno_administratora.htm");
        }
    }
}
