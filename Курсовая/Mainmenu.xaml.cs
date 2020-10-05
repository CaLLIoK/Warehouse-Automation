using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
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
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace StockRoom
{
    /// <summary>
    /// Логика взаимодействия для Mainmenu.xaml
    /// </summary>
    public partial class Mainmenu : System.Windows.Window
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StockRoom;Integrated Security=True";

        public Mainmenu()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginFormAdmin loginForm = new LoginFormAdmin();
            loginForm.Show();
            this.Close();
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            SparePartInformation sparePartInformation = new SparePartInformation();
            sparePartInformation.Show();
            this.Close();
        }
        
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchCriterion searchCriterion = new SearchCriterion();
            searchCriterion.Show();
            this.Close();
        }

        private void Statistic_Click(object sender, RoutedEventArgs e)
        {
            ShowStatistics showStatistics = new ShowStatistics();
            showStatistics.Show();
            this.Close();
        }

        private void ChangeUsersData_Click(object sender, RoutedEventArgs e)
        {
            ChangeUsersData changeUsersData = new ChangeUsersData();
            changeUsersData.Show();
            this.Close();
        }

        private void ChangeSparePartData_Click(object sender, RoutedEventArgs e)
        {
            ChangeSparePartsData changeSparePartsData = new ChangeSparePartsData();
            changeSparePartsData.Show();
            this.Close();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser user = new AddUser();
            user.Show();
            this.Close();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DeleteUser user = new DeleteUser();
            user.Show();
            this.Close();
        }

        private void AddSparePart_Click(object sender, RoutedEventArgs e)
        {
            AddSparePart sprPrt = new AddSparePart();
            sprPrt.Show();
            this.Close();
        }

        private void DeleteSparePart_Click(object sender, RoutedEventArgs e)
        {
            DeleteSparePart sprPrt = new DeleteSparePart();
            sprPrt.Show();
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "rukovodstvo_administratora.htm");
        }

        [DllImport("user32")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId0); // данный метод очищает процесс Excel.exe 

        private void CreateRepotButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Workbooks excelappworkbooks = null;
            Microsoft.Office.Interop.Excel.Workbook excelappworkbook = null;
            Microsoft.Office.Interop.Excel.Sheets excelsheets = null;
            Microsoft.Office.Interop.Excel.Worksheet excelworksheet = null;
            Microsoft.Office.Interop.Excel.Range excelcells = null;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // указываем путем сохранения рабочий стол
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Interactive = false;
            uint processId;
            GetWindowThreadProcessId((IntPtr)excelapp.Hwnd, out processId);
            try
            {
                excelappworkbooks = excelapp.Workbooks;
                excelappworkbook = excelapp.Workbooks.Open(System.IO.Path.GetFullPath(@"Шаблон.xlsx")); //открываем шаблон
                excelsheets = excelappworkbook.Worksheets;
                excelworksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelsheets.get_Item(1); // выбираем первый лист из excel док-та
                SqlConnection connection = new SqlConnection(connectionString);
                excelcells = excelworksheet.get_Range("E1");
                excelcells.Value2 = DateTime.Now.ToShortDateString().ToString();
                string query = "SELECT SparePartNumber, SparePartN, CarModelName, SparePartCreation, SparePartCount, SparePartCost, StateName FROM CarModel, SparePartName, SparePartStatus, SparePart WHERE SparePart.IDCarModel = CarModel.IDCarModel AND SparePart.IDStatus = SparePartStatus.IDStatus AND SparePart.IDSparePartN = SparePartName.IDSparePartN";
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                int i = 3;
                if (sqlDataReader.HasRows) // заполняем таблицу данными
                {
                    while (sqlDataReader.Read())
                    {
                        int k = 0;
                        for (int j = 1; j < 8; j++)
                        {
                            excelcells = (Microsoft.Office.Interop.Excel.Range)excelworksheet.Cells[i, j];
                            excelcells.Font.Name = "Times New Roman";
                            excelcells.Font.Size = 14;
                            excelcells.Borders.ColorIndex = 0;
                            excelcells.EntireColumn.AutoFit();
                            excelcells.EntireRow.AutoFit();
                            excelcells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;  
                            if (k == 3)
                            {
                                excelcells.Value2 = Convert.ToDateTime(sqlDataReader[k].ToString()).ToShortDateString();
                            }
                            else
                            {
                                excelcells.Value2 = sqlDataReader[k].ToString();
                            }
                            if (k < sqlDataReader.FieldCount)
                                ++k;
                        }                     
                        ++i;
                    }
                }
                else
                {
                    MessageBox.Show("На складе нет никаких автозапчастей.");
                    return;
                }
                sqlDataReader.Close();
                path += @"\Отчет за " + DateTime.Now.ToShortDateString() + ".xlsx";
                excelappworkbooks = excelapp.Workbooks;
                excelappworkbook = excelappworkbooks[1];
                excelappworkbook.SaveAs(path);
                connection.Close();
            }
            catch (Exception q)
            {
                MessageBox.Show(q.Message);
            }
            finally
            {
                Process.GetProcessById((int)processId).Kill();
            }
        }
    }
}