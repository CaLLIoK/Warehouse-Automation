using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Data.SqlClient;
using System.Data;

namespace StockRoom
{
    /// <summary>
    /// Логика взаимодействия для SparePartInformation.xaml
    /// </summary>
    public partial class SparePartInformation : Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";
        SqlDataAdapter dataAdapter;
        DataTable table;

        public SparePartInformation()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT SparePartNumber, CarModelName, SparePartN, StateName, SparePartCreation, SparePartCost, SparePartCount FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = SparePartName.IDSparePartN";
            dataAdapter = new SqlDataAdapter(query, connection);
            connection.Open();
            table = new DataTable();
            dataAdapter.Fill(table); // заполняем DataGrid данными запчастей на складе
            SparePartsGrid.ItemsSource = table.DefaultView;
            connection.Close();
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
                Mainmenu mainmenu = new Mainmenu();
                mainmenu.Show();
                this.Close();
                System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "vyvod_informatsii_ob_avtozapchastyakh_2.htm");
            }
            else
            {
                MainMenuUser mainMenuUser = new MainMenuUser();
                mainMenuUser.Show();
                this.Close();
                System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "vyvod_informatsii_ob_avtozapchastyakh_1.htm");
            }
        }
    }
}