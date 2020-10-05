using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для SearchCriterion.xaml
    /// </summary>
    public partial class SearchCriterion : Window
    {
        public SearchCriterion()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (criterion.Text == "Название запчасти")
            {
                if (searchCriterion.Text != CheckSparePart.CheckSparePartName(searchCriterion.Text))
                {
                    MessageBox.Show(CheckSparePart.CheckSparePartName(searchCriterion.Text));
                    return;
                }
            }
            else if (criterion.Text == "Название автомобиля")
            {
                if (searchCriterion.Text != CheckSparePart.CheckCarName(searchCriterion.Text))
                {
                    MessageBox.Show(CheckSparePart.CheckCarName(searchCriterion.Text));
                    return;
                }
            }
            else if (criterion.Text == "Дата изготовления")
            {
                if (searchCriterion.Text != CheckSparePart.CheckCreationDate(searchCriterion.Text))
                {
                    MessageBox.Show(CheckSparePart.CheckCreationDate(searchCriterion.Text));
                    return;
                }
            }
            else if (criterion.Text == "Стоимость одной детали")
            {
                if (searchCriterion.Text != CheckSparePart.CheckSparePartCost(searchCriterion.Text))
                {
                    MessageBox.Show(CheckSparePart.CheckSparePartCost(searchCriterion.Text));
                    return;
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали критерий поиска.");
                return;
            }
            StreamWriter writeCriterion = new StreamWriter("Criterion.txt"); // записываем критерий поиска в файл
            writeCriterion.Write(criterion.Text);
            writeCriterion.Close();

            StreamWriter writeSearchCriterion = new StreamWriter("SearchCriterion.txt"); // записываем искомую информацию в файл
            writeSearchCriterion.Write(searchCriterion.Text);
            writeSearchCriterion.Close();

            Search search = new Search();
            search.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void BackToMenu(object sender, RoutedEventArgs e)
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
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "poisk_avtozapchastej_2.htm");
            }
            else
            {
                System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
                System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "poisk_avtozapchastej_1.htm");
            }
        }
    }
}