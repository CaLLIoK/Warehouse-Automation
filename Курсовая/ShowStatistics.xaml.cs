using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Логика взаимодействия для ShowStatistics.xaml
    /// </summary>
    public partial class ShowStatistics : Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";
        DataTable table;

        public ShowStatistics()
        {
            InitializeComponent();
            StreamReader file = new StreamReader("UserLogin.txt");
            string login = file.ReadLine();
            file.Close();
            if (login == null) // если это администратор, то загружаем статистику заказов всех пользователей
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string query = "SELECT OrderNumber, UserLogin, SparePartN, CarModelName, SparePartOrder.SparePartCount, GeneralSum " +
                                                               "FROM Users, SparePartName, CarModel, SparePartOrder, SparePart " +
                                                               "WHERE SparePartOrder.UserCode = Users.UserCode AND SparePartOrder.SparePartNumber = SparePart.SparePartNumber AND CarModel.IDCarModel = SparePart.IDCarModel AND SparePart.IDSparePartN = SparePartName.IDSparePartN";
                connection.Open();
                table = new DataTable();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        table.Load(rdr);
                    }
                }
                StatisticGrid.ItemsSource = table.DefaultView;
                connection.Close();
            }
            else // если обычный пользователь, загружаем только его статиистику заказов
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string query = "SELECT OrderNumber, UserLogin, SparePartN, CarModelName, SparePartOrder.SparePartCount, GeneralSum " +
                                                              "FROM Users, SparePartName, CarModel, SparePartOrder, SparePart " +
                                                              "WHERE SparePartOrder.UserCode = Users.UserCode AND SparePartOrder.SparePartNumber = SparePart.SparePartNumber AND CarModel.IDCarModel = SparePart.IDCarModel AND SparePart.IDSparePartN = SparePartName.IDSparePartN AND SparePartOrder.UserCode = (SELECT UserCode FROM Users WHERE UserLogin = '" + login + "')";
                connection.Open();
                table = new DataTable();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        table.Load(rdr);
                    }
                }
                StatisticGrid.ItemsSource = table.DefaultView;
                connection.Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreamReader file = new StreamReader("UserLogin.txt");
            string login = file.ReadLine();
            file.Close();
            if (login == null)
            {
                Mainmenu mainmenu = new Mainmenu();
                mainmenu.Show();
                this.Close();
            }
            else
            {
                MainMenuUser mainMenuUser = new MainMenuUser();
                mainMenuUser.Show();
                this.Close();
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            StreamReader file = new StreamReader("UserLogin.txt");
            string login = file.ReadLine();
            file.Close();
            if (login == null)
            {
                System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "statistika_2.htm");
            }
            else
            {
                System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "statistika_1.htm");
            }
        }
    }
}