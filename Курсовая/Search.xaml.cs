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
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        DataTable table;

        public Search()
        {
            InitializeComponent();

            string connectionString = @"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockRoom; Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string criterion = string.Empty;
            StreamReader readCriterion = new StreamReader("Criterion.txt"); // считываем критерий поиска из файла
            criterion = readCriterion.ReadLine();
            readCriterion.Close();

            string searchCriterion = string.Empty;
            StreamReader readSearchCriterion = new StreamReader("SearchCriterion.txt"); // считываем интересующую информацию из файла
            searchCriterion = readSearchCriterion.ReadLine();
            readSearchCriterion.Close();

            if (criterion == "Название запчасти") // если критей поиска "Название запчасти", заполняем DataGrid всеми соответствиями 
            {
                string query = "SELECT SparePartNumber, CarModelName, SparePartN, StateName, SparePartCreation, SparePartCost, SparePartCount FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = (SELECT IDSparePartN FROM SparePartName WHERE SparePartN = '" + searchCriterion + "') AND SparePart.IDSparePartN = SparePartName.IDSparePartN";
                table = new DataTable();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        table.Load(rdr);
                    }                   
                }
                SparePartsGrid.ItemsSource = table.DefaultView;
            }
            else if (criterion == "Название автомобиля") // если критей поиска "Название автомобиля", заполняем DataGrid всеми соответствиями 
            {
                string query = "SELECT SparePartNumber, CarModelName, SparePartN, StateName, SparePartCreation, SparePartCost, SparePartCount FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDCarModel = (SELECT IDCarModel FROM CarModel WHERE CarModelName = '" + searchCriterion + "') AND SparePart.IDSparePartN = SparePartName.IDSparePartN";
                table = new DataTable();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        table.Load(rdr);
                    }
                }
                SparePartsGrid.ItemsSource = table.DefaultView;
            }
            else if (criterion == "Дата изготовления") // если критей поиска "Дата изготовления", заполняем DataGrid всеми соответствиями 
            {
                string query = "SELECT SparePartNumber, CarModelName, SparePartN, StateName, SparePartCreation, SparePartCost, SparePartCount FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = SparePartName.IDSparePartN AND SparePartCreation = '" + searchCriterion + "'";
                table = new DataTable();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        table.Load(rdr);
                    }
                }
                SparePartsGrid.ItemsSource = table.DefaultView;
            }
            else if (criterion == "Стоимость одной детали") // если критей поиска "Стоимость одной детали", заполняем DataGrid всеми соответствиями 
            {
                using (SqlConnection updateRow = new SqlConnection(connectionString))
                using (SqlCommand lastCommnd = updateRow.CreateCommand())
                {
                    lastCommnd.CommandText = "SELECT SparePartNumber, CarModelName, SparePartN, StateName, SparePartCreation, SparePartCost, SparePartCount FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = SparePartName.IDSparePartN AND SparePartCost = @cost";
                    lastCommnd.Parameters.AddWithValue("@cost", Convert.ToDouble(searchCriterion));

                    updateRow.Open();
                    table = new DataTable();
                    using (IDataReader rdr = lastCommnd.ExecuteReader())
                    {
                        table.Load(rdr);
                    }
                    SparePartsGrid.ItemsSource = table.DefaultView;
                    updateRow.Close();
                }
            }
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
                System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "vyvod_rezultata_poiska_2.htm");
            }
            else
            {
                System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "vyvod_rezultata_poiska_1.htm");
            }
        }
    }
}