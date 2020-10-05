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
    /// Логика взаимодействия для DeleteSparePart.xaml
    /// </summary>
    public partial class DeleteSparePart : Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";

        public DeleteSparePart()
        {
            InitializeComponent();
            string spareParts = string.Empty;
            ObservableCollection<int> codeList = new ObservableCollection<int>();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = @"SELECT SparePartNumber, SparePartN, CarModelName, SparePartCreation, SparePartCost, SparePartCount, StateName FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = SparePartName.IDSparePartN ORDER BY SparePartN";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader(); // заполняем данными combobox с кодами запчастей и listbox
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    spareParts = dataReader[0].ToString() + " - " + dataReader[1].ToString() + " на " + dataReader[2].ToString() + " - " + dataReader[6].ToString() + "\n     Изготовлено: " + dataReader[3].ToString() + "\n     Стоимость: " + dataReader[4].ToString() + " руб.  Количество: " + dataReader[5].ToString();
                    sparePartsList.Items.Add(spareParts);
                    codeList.Add(Convert.ToInt32(dataReader[0].ToString()));
                    var newList = from i in codeList orderby i select i;
                    deleteSparePart.ItemsSource = newList;
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteSparePart.Text != string.Empty)
            {
                using (SqlConnection deleteRow = new SqlConnection(connectionString))
                using (SqlCommand lastCommnd = deleteRow.CreateCommand()) // удаляем запчасть
                {
                    lastCommnd.CommandText = "DELETE FROM SparePart WHERE SparePartNumber = @number";

                    lastCommnd.Parameters.AddWithValue("@number", deleteSparePart.Text);

                    deleteRow.Open();
                    lastCommnd.ExecuteNonQuery();
                    deleteRow.Close();
                }
                MessageBox.Show("Автозапчасть удалена.");
                deleteSparePart.SelectedIndex = -1; // убираем выбранный код из combobox
                sparePartsList.Items.Clear(); 
                string spareParts = string.Empty;
                ObservableCollection<int> codeList = new ObservableCollection<int>();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query = @"SELECT SparePartNumber, SparePartN, CarModelName, SparePartCreation, SparePartCost, SparePartCount, StateName FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = SparePartName.IDSparePartN";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows) // обновляем данные combobox и listbox
                {
                    while (dataReader.Read())
                    {
                        spareParts = dataReader[0].ToString() + " - " + dataReader[1].ToString() + " на " + dataReader[2].ToString() + " - " + dataReader[6].ToString() + "\n     Изготовлено: " + dataReader[3].ToString() + "\n     Стоимость: " + dataReader[4].ToString() + " руб.  Количество: " + dataReader[5].ToString();
                        sparePartsList.Items.Add(spareParts);
                        codeList.Add(Convert.ToInt32(dataReader[0].ToString()));
                        var newList = from i in codeList orderby i select i;
                        deleteSparePart.ItemsSource = newList;
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не указали код автозапчасти, которую собираетесь удалить.");
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
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "udalenie_avtozapchasti.htm");
        }
    }
}