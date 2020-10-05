using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ChangeSparePartsData.xaml
    /// </summary>
    public partial class ChangeSparePartsData : Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";
        DataTable table;

        public ChangeSparePartsData()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT SparePartNumber, CarModelName, SparePartN, StateName, SparePartCreation, SparePartCost, SparePartCount FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = SparePartName.IDSparePartN";
            connection.Open();
            table = new DataTable();
            using (SqlCommand cmd = new SqlCommand(query, connection)) //загружаем данные в DataGrid
            {
                using (IDataReader rdr = cmd.ExecuteReader())
                {
                    table.Load(rdr);
                }
            }
            SparePartsGrid.ItemsSource = table.DefaultView;
            connection.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Update update = new Update();
            update.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mainmenu mainmenu = new Mainmenu();
            mainmenu.Show();
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "izmenenie_dannykh_avtozapchastej.htm");
        }
    }
}