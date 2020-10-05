using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MainMenuUser.xaml
    /// </summary>
    public partial class MainMenuUser : Window
    {
        public MainMenuUser()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
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
            SearchCriterion search = new SearchCriterion();
            search.Show();
            this.Close();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            order.Show();
            this.Close();
        }


        private void Statistic_Click(object sender, RoutedEventArgs e)
        {
            ShowStatistics showStatistics = new ShowStatistics();
            showStatistics.Show();
            this.Close();
        }

        private void ChangeAccountData_Click(object sender, RoutedEventArgs e)
        {
            ChangeAccountData changeAccountData = new ChangeAccountData();
            changeAccountData.Show();
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "rukovodstvo_polzovatelya.htm");
        }
    }
}