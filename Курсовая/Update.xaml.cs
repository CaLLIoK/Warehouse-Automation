using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";

        public Update()
        {
            InitializeComponent();
            List<int> codeList = new List<int>();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = @"SELECT SparePartNumber FROM SparePart";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.HasRows) // заполняем combobox кодами запчастей
            {
                while (dataReader.Read())
                {
                    codeList.Add(Convert.ToInt32(dataReader[0].ToString()));
                    searchCriterion.ItemsSource = codeList;
                }
            }
            connection.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void SaveChanges(object sender, RoutedEventArgs e)
        { 
            if (criterion.Text != string.Empty && searchCriterion.Text != string.Empty)
            {
                int sprPrtCode = Convert.ToInt32(searchCriterion.Text);
                if (criterion.Text == "Название запчасти")
                {
                    if (changingCriterion.Text != CheckSparePart.CheckSparePartName(changingCriterion.Text))
                    {
                        MessageBox.Show(CheckSparePart.CheckSparePartName(changingCriterion.Text));
                        return;
                    }
                    string selectIdSprPrt = @"SELECT IDSparePartN FROM SparePartName WHERE SparePartN = '" + changingCriterion.Text + "'";
                    int idSparePartN = 0;
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(selectIdSprPrt, sqlConnection);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows) // находим id-шник нового названия запчасти в базе
                    {
                        while (sqlDataReader.Read())
                        {
                            idSparePartN = Convert.ToInt32(sqlDataReader[0].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("В базе нет данных с таким названием запчасти.");
                        return;
                    }
                    sqlDataReader.Close();
                    using (SqlConnection updateRow = new SqlConnection(connectionString))
                    using (SqlCommand lastCommnd = updateRow.CreateCommand()) // обновляем информацию
                    {
                        lastCommnd.CommandText = "UPDATE SparePart SET IDSparePartN = @name WHERE SparePartNumber = @number";

                        lastCommnd.Parameters.AddWithValue("@name", idSparePartN);
                        lastCommnd.Parameters.AddWithValue("@number", sprPrtCode);

                        updateRow.Open();
                        lastCommnd.ExecuteNonQuery();
                        updateRow.Close();
                    }
                    sqlConnection.Close();
                }
                else if (criterion.Text == "Название автомобиля")
                {
                    if (changingCriterion.Text != CheckSparePart.CheckCarName(changingCriterion.Text))
                    {
                        MessageBox.Show(CheckSparePart.CheckCarName(changingCriterion.Text));
                        return;
                    }
                    string selectIdSprPrt = @"SELECT IDCarModel FROM CarModel WHERE CarModelName = '" + changingCriterion.Text + "'";
                    int idCarModel = 0;
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(selectIdSprPrt, sqlConnection);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows) // находим id-шник нового названия автомобиля в базе
                    {
                        while (sqlDataReader.Read())
                        {
                            idCarModel = Convert.ToInt32(sqlDataReader[0].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("В базе нет данных с таким названием автомобиля.");
                        return;
                    }
                    sqlDataReader.Close();
                    using (SqlConnection updateRow = new SqlConnection(connectionString))
                    using (SqlCommand lastCommnd = updateRow.CreateCommand()) // обновляем информацию
                    {
                        lastCommnd.CommandText = "UPDATE SparePart SET IDCarModel = @name WHERE SparePartNumber = @number";

                        lastCommnd.Parameters.AddWithValue("@name", idCarModel);
                        lastCommnd.Parameters.AddWithValue("@number", sprPrtCode);

                        updateRow.Open();
                        lastCommnd.ExecuteNonQuery();
                        updateRow.Close();
                    }
                    sqlConnection.Close();
                }
                else if (criterion.Text == "Дата изготовления")
                {
                    if (changingCriterion.Text != CheckSparePart.CheckCreationDate(changingCriterion.Text))
                    {
                        MessageBox.Show(CheckSparePart.CheckCreationDate(changingCriterion.Text));
                        return;
                    }
                    using (SqlConnection updateRow = new SqlConnection(connectionString))
                    using (SqlCommand lastCommnd = updateRow.CreateCommand()) // обновляем информацию
                    {
                        lastCommnd.CommandText = "UPDATE SparePart SET SparePartCreation = @date WHERE SparePartNumber = @number";
                        lastCommnd.Parameters.AddWithValue("@number", sprPrtCode);
                        lastCommnd.Parameters.AddWithValue("@date", changingCriterion.Text);

                        updateRow.Open();
                        lastCommnd.ExecuteNonQuery();
                        updateRow.Close();
                    }
                }
                else if (criterion.Text == "Стоимость одной детали")
                {
                    if (changingCriterion.Text != CheckSparePart.CheckSparePartCost(changingCriterion.Text))
                    {
                        MessageBox.Show(CheckSparePart.CheckSparePartCost(changingCriterion.Text));
                        return;
                    }
                    using (SqlConnection updateRow = new SqlConnection(connectionString))
                    using (SqlCommand lastCommnd = updateRow.CreateCommand()) // обновляем информацию
                    {
                        lastCommnd.CommandText = "UPDATE SparePart SET SparePartCost = @cost WHERE SparePartNumber = @number";                       
                        lastCommnd.Parameters.AddWithValue("@number", sprPrtCode);
                        lastCommnd.Parameters.AddWithValue("@cost", Convert.ToDouble(changingCriterion.Text));
                        
                        updateRow.Open();
                        lastCommnd.ExecuteNonQuery();
                        updateRow.Close();
                    }
                }
                else if (criterion.Text == "Количество")
                {
                    if (changingCriterion.Text != CheckSparePart.CheckSparePartCount(changingCriterion.Text))
                    {
                        MessageBox.Show(CheckSparePart.CheckSparePartCount(changingCriterion.Text));
                        return;
                    }
                    using (SqlConnection updateRow = new SqlConnection(connectionString)) // обновляем информацию
                    using (SqlCommand lastCommnd = updateRow.CreateCommand())
                    {
                        lastCommnd.CommandText = "UPDATE SparePart SET SparePartCount = @count, IDStatus = (SELECT IDStatus FROM SparePartStatus WHERE StateName  = @status) WHERE SparePartNumber = @number";
                        if (Convert.ToInt32(changingCriterion.Text) == 0) // если новое кол-во 0, меняем статус на "нет в наличии"
                        {
                            lastCommnd.Parameters.AddWithValue("@number", sprPrtCode);
                            lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(changingCriterion.Text));
                            lastCommnd.Parameters.AddWithValue("@status", "Нет в наличии");
                        }
                        else
                        {
                            lastCommnd.Parameters.AddWithValue("@number", sprPrtCode);
                            lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(changingCriterion.Text));
                            lastCommnd.Parameters.AddWithValue("@status", "Есть в наличии");
                        }

                        updateRow.Open();
                        lastCommnd.ExecuteNonQuery();
                        updateRow.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали данные для изменения.");
                return;
            }
            MessageBoxResult mboxResult = MessageBox.Show("Данные обновлены. Желаете изменить что-нибудь еще?", "Предупреждение", MessageBoxButton.YesNo);
            if (mboxResult == MessageBoxResult.No)
            {
                ChangeSparePartsData changeSparePartsData = new ChangeSparePartsData();
                changeSparePartsData.Show();
                this.Close();
            }
        }

        private void BackToWindow(object sender, RoutedEventArgs e)
        {
            ChangeSparePartsData changeSparePartsData = new ChangeSparePartsData();
            changeSparePartsData.Show();
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "obnovlenie_dannykh_avtozapchastej.htm");
        }
    }
}