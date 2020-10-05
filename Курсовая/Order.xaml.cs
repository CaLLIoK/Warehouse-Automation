using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
using Document = Microsoft.Office.Interop.Word.Document;

namespace StockRoom
{
    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : System.Windows.Window
    {
        public Order()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void MakeOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SparePartName.Text != CheckSparePart.CheckSparePartName(SparePartName.Text))
            {
                MessageBox.Show(CheckSparePart.CheckSparePartName(SparePartName.Text));
                return;
            }

            if (CarName.Text != CheckSparePart.CheckCarName(CarName.Text))
            {
                MessageBox.Show(CheckSparePart.CheckCarName(CarName.Text));
                return;
            }

            if (SparePartCount.Text != CheckSparePart.CheckSparePartCount(SparePartCount.Text))
            {
                MessageBox.Show(CheckSparePart.CheckSparePartCount(SparePartCount.Text));
                return;
            }

            int sparePartID = 0;
            int carModelID = 0;

            string myConnectionString = @"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockRoom; Integrated Security=True";
            SqlConnection connection = new SqlConnection(myConnectionString);
            connection.Open();
            string findOrderSparePartName = "SELECT IDSparePartN FROM SparePartName WHERE SparePartN = '" + SparePartName.Text + "'";
            SqlCommand findSparePartName = new SqlCommand(findOrderSparePartName, connection);
            SqlDataReader sparePartNameReader = findSparePartName.ExecuteReader();
            if (sparePartNameReader.Read()) // ищем id-шник названия запчасти в базе
            {
                sparePartID = Convert.ToInt32(sparePartNameReader[0].ToString());
                sparePartNameReader.Close();
            }
            else
            {
                MessageBox.Show("Такой автозапчасти нет на складе.");
                return;
            }
            sparePartNameReader.Close();

            string findOrderCarName = "SELECT IDCarModel FROM CarModel WHERE CarModelName = '" + CarName.Text + "'";
            SqlCommand findCarNameCommand = new SqlCommand(findOrderCarName, connection);
            SqlDataReader carNameReader = findCarNameCommand.ExecuteReader();
            if (carNameReader.Read()) // ищем id-шник названия автомобиля в базе
            {
                carModelID = Convert.ToInt32(carNameReader[0].ToString());
                carNameReader.Close();
            }
            else
            {
                MessageBox.Show("Автозапчасти для такой марки автомобиля нет.");
                return;
            }

            StreamReader file = new StreamReader("UserLogin.txt");
            string login = file.ReadLine();
            file.Close();

            int userCode = 0;
            string findUserCode = "SELECT UserCode FROM Users WHERE UserLogin = '" + login + "'";
            SqlCommand findUC = new SqlCommand(findUserCode, connection);
            SqlDataReader UCReader = findUC.ExecuteReader();
            if (UCReader.HasRows)  // получаем id-шник пользователя
            {
                while (UCReader.Read())
                {
                    userCode = Convert.ToInt32(UCReader[0].ToString());
                }
            }
            UCReader.Close();

            double sparePartCost = 0;
            int currentCount = 0;
            int sparePartNumber = 0;
            string findOrderData = "SELECT SparePartNumber, SparePartCount, SparePartCost FROM SparePart WHERE IDSparePartN = " + sparePartID + " AND IDCarModel = " + carModelID + "";
            SqlCommand findData = new SqlCommand(findOrderData, connection);
            SqlDataReader dataReader = findData.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read()) // берем данные запчасти со склада: номер, кол-во, стоимость
                {
                    sparePartNumber = Convert.ToInt32(dataReader[0].ToString());
                    currentCount = Convert.ToInt32(dataReader[1].ToString());
                    sparePartCost = Convert.ToDouble(dataReader[2].ToString());
                }

                if (currentCount < Convert.ToInt32(SparePartCount.Text)) // если кол-во запчастей на складе меньше кол-ва запчастей в заказе
                {
                    MessageBox.Show("Такого количества указанной автозапчасти на складе нет.");
                    return;
                }
                else if (currentCount > Convert.ToInt32(SparePartCount.Text)) // если кол-во запчастей на складе больше кол-ва запчастей в заказе
                {
                    using (SqlConnection changeCount = new SqlConnection(myConnectionString))
                    using (SqlCommand lastCommnd = changeCount.CreateCommand()) // уменьшаем кол-во запчастей на складе
                    {
                        lastCommnd.CommandText = "UPDATE SparePart SET SparePartCount = SparePartCount - @count WHERE SparePartNumber = " + sparePartNumber + "";
                        lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(SparePartCount.Text));

                        changeCount.Open();
                        lastCommnd.ExecuteNonQuery();
                        changeCount.Close();
                    }

                    using (SqlConnection insertOrderData = new SqlConnection(myConnectionString))
                    using (SqlCommand lastCommnd = insertOrderData.CreateCommand()) // записываем данные заказа в базу данных
                    {
                        lastCommnd.CommandText = "INSERT SparePartOrder (UserCode, SparePartNumber, SparePartCount, GeneralSum) VALUES (@UC, @sprptNum, @count, @sum)";
                        lastCommnd.Parameters.AddWithValue("@UC", userCode);
                        lastCommnd.Parameters.AddWithValue("@sprptNum", sparePartNumber);
                        lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(SparePartCount.Text));
                        lastCommnd.Parameters.AddWithValue("@sum", Convert.ToDouble(SparePartCount.Text) * sparePartCost);

                        insertOrderData.Open();
                        lastCommnd.ExecuteNonQuery();
                        insertOrderData.Close();
                    }
                }
                else if (currentCount == Convert.ToInt32(SparePartCount.Text)) // если кол-во запчастей на складе равно кол-ву запчастей в заказе
                {
                    using (SqlConnection changeCount = new SqlConnection(myConnectionString))
                    using (SqlCommand lastCommnd = changeCount.CreateCommand())
                    {
                        lastCommnd.CommandText = "UPDATE SparePart SET SparePartCount = SparePartCount - @count, IDStatus = (SELECT IDStatus FROM SparePartStatus WHERE StateName = @status) WHERE SparePartNumber = " + sparePartNumber + "";
                        lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(SparePartCount.Text));
                        lastCommnd.Parameters.AddWithValue("@status", "Нет в наличии");

                        changeCount.Open();
                        lastCommnd.ExecuteNonQuery();
                        changeCount.Close();
                    }

                    using (SqlConnection insertOrderData = new SqlConnection(myConnectionString))
                    using (SqlCommand lastCommnd = insertOrderData.CreateCommand()) // записываем данные заказа в базу данных
                    {
                        lastCommnd.CommandText = "INSERT SparePartOrder (UserCode, SparePartNumber, SparePartCount, GeneralSum) VALUES (@UC, @sprptNum, @count, @sum)";
                        lastCommnd.Parameters.AddWithValue("@UC", userCode);
                        lastCommnd.Parameters.AddWithValue("@sprptNum", sparePartNumber);
                        lastCommnd.Parameters.AddWithValue("@count", Convert.ToInt32(SparePartCount.Text));
                        lastCommnd.Parameters.AddWithValue("@sum", Convert.ToDouble(SparePartCount.Text) * sparePartCost);

                        insertOrderData.Open();
                        lastCommnd.ExecuteNonQuery();
                        insertOrderData.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет подходящей автозапчасти для указанного автомобиля.");
                return;
            }
            dataReader.Close();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application(); // открываем Word
            try
            {
                Document doc = app.Documents.Open(System.IO.Path.GetFullPath(@"ШаблонЗаказ.docx")); // открыаем шаблон
                ReplaseWord("{OrderDate}", DateTime.Now.ToShortDateString(), doc); // записываем в метку дату заказа
                string query = "SELECT OrderNumber, CarModelName, SparePartN, SparePartCost, SparePartOrder.SparePartCount, UserName, UserSurname, UserLogin, GeneralSum FROM SparePartOrder, CarModel, SparePartName, SparePart, Users WHERE SparePartOrder.UserCode = Users.UserCode AND SparePartOrder.UserCode = " + userCode + " AND SparePartOrder.SparePartNumber = SparePart.SparePartNumber AND SparePartOrder.SparePartNumber = " + sparePartNumber + " AND SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDSparePartN = SparePartName.IDSparePartN AND SparePartOrder.SparePartCount = " + Convert.ToInt32(SparePartCount.Text) + "";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows) // записываем в метки данные заказа
                {
                    while (sqlDataReader.Read())
                    {
                        ReplaseWord("{OrderNumber}", sqlDataReader[0].ToString() + "\n", doc);
                        ReplaseWord("{SparePartN}", sqlDataReader[2].ToString() + "\n", doc);
                        ReplaseWord("{CarModelName}", sqlDataReader[1].ToString() + "\n", doc);
                        ReplaseWord("{SparePartCost}", sqlDataReader[3].ToString() + "\n", doc);
                        ReplaseWord("{SparePartCount}", sqlDataReader[4].ToString() + "\n", doc);
                        ReplaseWord("{UserName}", sqlDataReader[5].ToString() + "\n", doc);
                        ReplaseWord("{UserSurname}", sqlDataReader[6].ToString() + "\n", doc);
                        ReplaseWord("{UserLogin}", sqlDataReader[7].ToString() + "\n", doc);
                        ReplaseWord("{GeneralSum}", sqlDataReader[8].ToString() + "\n", doc);
                        path += @"\Отчет по заказу № " + sqlDataReader[0].ToString() + ".docx";                     
                    }
                }
                doc.SaveAs2(path);
                sqlDataReader.Close();
                doc.Close();
                connection.Close();
            }
            catch (Exception q)
            {
                app.Quit();
                MessageBox.Show(q.Message);
            }
            finally
            {
                app.Quit();
            }

            MessageBoxResult mboxResult = MessageBox.Show("Заказ успешно выполнен. Желаете заказать что-нибудь еще?", "Предупреждение", MessageBoxButton.YesNo);
            if (mboxResult == MessageBoxResult.No)
            {
                MainMenuUser mainMenuUser = new MainMenuUser();
                mainMenuUser.Show();
                this.Close();
            }
            connection.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainMenuUser mainMenuUser = new MainMenuUser();
            mainMenuUser.Show();
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "oformlenie_zakaza.htm");
        }

        private void ReplaseWord(string StubReplase, string Text, Document Doc)
        {
            var range = Doc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: StubReplase, ReplaceWith: Text);
        }
    }
}