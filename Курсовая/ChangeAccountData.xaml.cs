using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для ChangeAccountData.xaml
    /// </summary>
    public partial class ChangeAccountData : Window
    {
        public ChangeAccountData()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreamReader readLogin = new StreamReader("UserLogin.txt");
            string currentUserLogin = readLogin.ReadLine();
            readLogin.Close();

            string connectionString = @"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockRoom; Integrated Security=True";
            if (loginField.Text != "") //проверка логина на соответствие
            {
                if (loginField.Text.Length > 2 && loginField.Text.Length <= 20)
                {
                    char[] loginArray = loginField.Text.ToCharArray();
                    for (int i = 0; i < loginArray.Length; i++)
                    {
                        if (!char.IsLetter(loginArray[i]) && !char.IsDigit(loginArray[i]) && loginArray[i] != '_')
                        {
                            MessageBox.Show("Вы указали в логине недопустимые символы.");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Допустимая длина логина 3-20 символов.");
                    return;
                }
                
                using (SqlConnection changeLogin = new SqlConnection(connectionString))
                using (SqlCommand command = changeLogin.CreateCommand()) // обновляем логин пользователя
                {
                    StreamWriter writeLogin = new StreamWriter("UserLogin.txt");
                    writeLogin.Write(loginField.Text);
                    writeLogin.Close();
                    command.CommandText = "UPDATE Users SET UserLogin = '" + loginField.Text + "' WHERE UserLogin = '" + currentUserLogin + "'";
                    changeLogin.Open();
                    command.ExecuteNonQuery();
                    changeLogin.Close();
                    currentUserLogin = loginField.Text;
                }
            }

            if (passwordField.Password.ToString() != "") //проверка пароля на соответствие
            {
                if (passwordField.Password.ToString().Length > 2 && passwordField.Password.ToString().Length <= 20)
                {
                    char[] passwordArray = passwordField.Password.ToString().ToCharArray();
                    for (int i = 0; i < passwordArray.Length; i++)
                    {
                        if (!char.IsLetter(passwordArray[i]) && !char.IsDigit(passwordArray[i]) && passwordArray[i] != '_' && passwordArray[i] != '*')
                        {
                            MessageBox.Show("Вы указали в пароле недопустимые символы.");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Допустимая длина пароля 3-20 символов.");
                    return;
                }

                using (SqlConnection changePassword = new SqlConnection(connectionString))
                using (SqlCommand command = changePassword.CreateCommand()) // обновляем пароль пользователя
                {
                    command.CommandText = "UPDATE Users SET UserPassword = '" + passwordField.Password.ToString() + "' WHERE UserLogin = '" + currentUserLogin + "'";
                    changePassword.Open();
                    command.ExecuteNonQuery();
                    changePassword.Close();
                }
            }

            if (nameField.Text != "")  //проверка имени на соответствие
            {
                if (nameField.Text.Length > 2 && nameField.Text.Length <= 20)
                {
                    char[] nameArray = nameField.Text.ToCharArray();
                    for (int i = 0; i < nameArray.Length; i++)
                    {
                        if (!char.IsLetter(nameArray[i]) && nameArray[i] != '-')
                        {
                            MessageBox.Show("Вы указали в имени недопустимые символы.");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Допустимая длина имени 3-20 символов.");
                    return;
                }

                using (SqlConnection changeName = new SqlConnection(connectionString))
                using (SqlCommand command = changeName.CreateCommand()) // обновляем имя пользователя
                {
                    command.CommandText = "UPDATE Users SET UserName = '" + nameField.Text + "' WHERE UserLogin = '" + currentUserLogin + "'";
                    changeName.Open();
                    command.ExecuteNonQuery();
                    changeName.Close();
                }
            }

            if (surnamField.Text != "") //проверка фамилии на соответствие
            {
                if (surnamField.Text.Length > 2 && surnamField.Text.Length <= 20)
                {
                    char[] surnameArray = surnamField.Text.ToCharArray();
                    for (int i = 0; i < surnameArray.Length; i++)
                    {
                        if (!char.IsLetter(surnameArray[i]) && surnameArray[i] != '-')
                        {
                            MessageBox.Show("Вы указали в фамилии недопустимые символы.");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Допустимая длина фамилии 3-20 символов.");
                    return;
                }

                using (SqlConnection changeSurname = new SqlConnection(connectionString))
                using (SqlCommand command = changeSurname.CreateCommand()) //обновляем фамилию пользователя
                {
                    command.CommandText = "UPDATE Users SET UserSurname = '" + surnamField.Text + "' WHERE UserLogin = '" + currentUserLogin + "'";
                    changeSurname.Open();
                    command.ExecuteNonQuery();
                    changeSurname.Close();
                }
            }
            MessageBoxResult mboxResult = MessageBox.Show("Изменения сохранены. Желаете изменить что-нибудь еще?", "Предупреждение", MessageBoxButton.YesNo);
            if (mboxResult == MessageBoxResult.No)
            {
                MainMenuUser mainMenuUser = new MainMenuUser();
                mainMenuUser.Show();
                this.Close();
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.HelpNavigator navigator = System.Windows.Forms.HelpNavigator.Topic;
            System.Windows.Forms.Help.ShowHelp(null, "help.chm", navigator, "redaktirovanie_dannykh_akkaunta.htm");
        }
    }
}