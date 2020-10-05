using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для AddSparePart.xaml
    /// </summary>
    public partial class AddSparePart : Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";

        public AddSparePart()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int sparePartNameID = 0;
            int carModelID = 0;
            int stateID = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            if (sparePartName.Text != CheckSparePart.CheckSparePartName(sparePartName.Text))
            {
                MessageBox.Show(CheckSparePart.CheckSparePartName(sparePartName.Text));
                return;
            }
            string findSparePart = "SELECT IDSparePartN FROM SparePartName WHERE SparePartN = '" + sparePartName.Text + "'";
            SqlCommand command = new SqlCommand(findSparePart, connection);
            SqlDataReader firstReader = command.ExecuteReader(); //ищем id-шник названия детальки, если такое имеется
            if (firstReader.Read())
            {
                sparePartNameID = Convert.ToInt32(firstReader[0].ToString());
            }
            else //если нет, тогда это название добавляем
            {
                string AddSparePartName = "INSERT INTO SparePartName (SparePartN) VALUES ('" + sparePartName.Text + "')";
                SqlCommand AddCommnd = new SqlCommand(AddSparePartName, connection);
                firstReader.Close();
                AddCommnd.ExecuteNonQuery();
            }
            firstReader.Close();

            if (carName.Text != CheckSparePart.CheckCarName(carName.Text))
            {
                MessageBox.Show(CheckSparePart.CheckCarName(carName.Text));
                return;
            }
            string findCar = "SELECT IDCarModel FROM CarModel WHERE CarModelName = '" + carName.Text + "'";
            SqlCommand command1 = new SqlCommand(findCar, connection);
            SqlDataReader secondReader = command1.ExecuteReader(); //ищем id-шник названия автомобиля, если такое имеется
            if (secondReader.Read())
            {
                carModelID = Convert.ToInt32(secondReader[0].ToString());
                secondReader.Close();
            }
            else //если нет, тогда это название добавляем
            {
                string AddCarModelName = "INSERT INTO CarModel (CarModelName) VALUES ('" + carName.Text + "')";
                SqlCommand AddCommnd = new SqlCommand(AddCarModelName, connection);
                secondReader.Close();
                AddCommnd.ExecuteNonQuery();
            }

            if (creationDate.Text != CheckSparePart.CheckCreationDate(creationDate.Text))
            {
                MessageBox.Show(CheckSparePart.CheckCreationDate(creationDate.Text));
                return;
            }

            string state = string.Empty;
            if (count.Text != CheckSparePart.CheckSparePartCount(count.Text))
            {
                MessageBox.Show(CheckSparePart.CheckSparePartCount(count.Text));
                return;
            }
            else
            {
                if (Convert.ToInt32(count.Text) > 0)
                {
                    state = "Есть в наличии";
                }
                else
                {
                    if (Convert.ToInt32(count.Text) == 0)
                    {
                        state = "Нет в наличии";
                    }
                }
            }              

            string findStateID = "SELECT IDStatus FROM SparePartStatus WHERE StateName = '" + state + "'";
            SqlCommand FindStateIDCommand = new SqlCommand(findStateID, connection);
            SqlDataReader readIDState = FindStateIDCommand.ExecuteReader(); // берем id-шник названия статуса
            if (readIDState.Read())
            {
                stateID = Convert.ToInt32(readIDState[0].ToString());
                readIDState.Close();
            }    

            if (cost.Text != CheckSparePart.CheckSparePartCost(cost.Text))
            {
                MessageBox.Show(CheckSparePart.CheckSparePartCost(cost.Text));
                return;
            }  

            string ID = "SELECT IDSparePartN, IDCarModel FROM SparePartName, CarModel WHERE SparePartN = '" + sparePartName.Text + "' AND CarModelName = '" + carName.Text + "'";
            SqlCommand sqlCommand = new SqlCommand(ID, connection);
            SqlDataReader readID = sqlCommand.ExecuteReader();
            if (readID.HasRows)
            {
                while (readID.Read())
                {
                    sparePartNameID = Convert.ToInt32(readID[0].ToString());
                    carModelID = Convert.ToInt32(readID[1].ToString());
                }
            }
            readID.Close();

            double currentCost = 0;

            string findReplays = "SELECT SparepartCost, IDSparePartN, IDCarModel, SparePartCreation FROM SparePart WHERE IDSparePartN = " + sparePartNameID + "AND IDCarModel = " + carModelID + "AND SparePartCreation = '" + creationDate.Text + "'";
            SqlCommand findReplaysCommand = new SqlCommand(findReplays, connection);
            SqlDataReader replaysReader = findReplaysCommand.ExecuteReader();
            if (replaysReader.HasRows) //проверка на наличие данных. если соответствия есть - будем обновлять кол-во и стоимость
            {
                while (replaysReader.Read()) //переходы к последующим записям
                {
                    currentCost = Convert.ToDouble(replaysReader[0].ToString()); //текущая стоимость
                    if (currentCost < Convert.ToDouble(cost.Text)) //если текущая стоимость меньше той стоимости, которую указали при добавлении новой детали, указываем новую
                    {
                        using (SqlConnection changeSum = new SqlConnection(connectionString))
                        using (SqlCommand lastCommnd = changeSum.CreateCommand())
                        {
                            lastCommnd.CommandText = "UPDATE SparePart SET SparePartCost = @sum, SparePartCount = SparePartCount + @count, IDStatus = (SELECT IDStatus FROM SparePartStatus WHERE StateName  = 'Есть в наличии') WHERE IDSparePartN = " + sparePartNameID + "AND IDCarModel = " + carModelID + "AND SparePartCreation = '" + creationDate.Text + "'";
                            lastCommnd.Parameters.AddWithValue("@sum", Convert.ToDouble(cost.Text));
                            lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(count.Text));

                            changeSum.Open();
                            lastCommnd.ExecuteNonQuery();
                            changeSum.Close();
                        }
                    }
                    else //если текущая стоимость больше той стоимости, которую указали при добавлении новой детали, оставляем старую
                    {
                        using (SqlConnection changeSum = new SqlConnection(connectionString))
                        using (SqlCommand lastCommnd = changeSum.CreateCommand())
                        {
                            lastCommnd.CommandText = "UPDATE SparePart SET SparePartCount = SparePartCount + @count, IDStatus = (SELECT IDStatus FROM SparePartStatus WHERE StateName  = 'Есть в наличии') WHERE IDSparePartN = " + sparePartNameID + "AND IDCarModel = " + carModelID + "AND SparePartCreation = '" + creationDate.Text + "'";
                            lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(count.Text));

                            changeSum.Open();
                            lastCommnd.ExecuteNonQuery();
                            changeSum.Close();
                        }
                    }
                }
            }
            else //если соответствий нет - добавляем новую запчасть на склад
            {
                using (SqlConnection changeSum = new SqlConnection(connectionString))
                using (SqlCommand lastCommnd = changeSum.CreateCommand())
                {
                    lastCommnd.CommandText = "INSERT INTO SparePart (IDSparePartN, IDCarModel, IDStatus, SparePartCreation, SparePartCost, SparePartCount) VALUES (@name, @car, @state, @date, @sum, @count)";

                    lastCommnd.Parameters.AddWithValue("@name", Convert.ToInt32(sparePartNameID));
                    lastCommnd.Parameters.AddWithValue("@car", Convert.ToInt32(carModelID));
                    lastCommnd.Parameters.AddWithValue("@state", Convert.ToInt32(stateID));
                    lastCommnd.Parameters.AddWithValue("@date", creationDate.Text);
                    lastCommnd.Parameters.AddWithValue("@sum", Convert.ToDouble(cost.Text));
                    lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(count.Text));

                    changeSum.Open();
                    lastCommnd.ExecuteNonQuery();
                    changeSum.Close();
                }
            }
            MessageBox.Show("Автозапчасть добавлена на склад.");
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
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "dobavlenie_avtozapchasti.htm");
        }
    }
}