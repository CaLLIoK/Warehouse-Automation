using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockRoom
{
    [Table("Users")]
    public class User
    {
        private string userLogin;
        private string userPassword;
        private bool administratorState;
        private string userSurname;
        private string userName;

        [Key]
        public int UserCode { get; set; }
        public string UserLogin
        {
            get => userLogin;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Логин не может быть пустым.");
                    }
                    else
                    {
                        if (value.Length > 2 && value.Length <= 20)
                        {
                            char[] loginArray = value.ToCharArray();
                            for (int i = 0; i < loginArray.Length; i++)
                            {
                                if ((loginArray[i] < 'a' || loginArray[i] > 'z') && loginArray[i] != '_' && (loginArray[i] < '0' || loginArray[i] > '9') && (loginArray[i] < 'A' || loginArray[i] > 'Z'))
                                {
                                    throw new Exception("Вы указали в логине недопустимые символы.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Допустимая длина логина 3-20 символов.");
                        }
                    }
                    userLogin = value;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }               
            }
        }
        public string UserPassword
        {
            get => userPassword;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Пароль не может быть пустым.");
                    }
                    else
                    {
                        if (value.Length > 2 && value.Length <= 20)
                        {
                            char[] passwordArray = value.ToCharArray();
                            for (int i = 0; i < passwordArray.Length; i++)
                            {
                                if ((passwordArray[i] < 'a' || passwordArray[i] > 'z') && passwordArray[i] != '_' && (passwordArray[i] < '0' || passwordArray[i] > '9') && (passwordArray[i] < 'A' || passwordArray[i] > 'Z'))
                                {
                                    throw new Exception("Вы указали в пароле недопустимые символы.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Допустимая длина пароля 3-20 символов.");
                        }
                    }
                    userPassword = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public bool AdministratorState
        {
            get => administratorState;
            set
            {
                try
                {
                    if (value == true || value == false)
                    {
                        char[] statusArray = value.ToString().ToCharArray();
                        for (int i = 0; i < statusArray.Length; i++)
                        {
                            if (!char.IsLetter(statusArray[i]))
                            {
                                throw new Exception("Вы указали в статусе недопустимые символы.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Статус администратора указан неверно. Нужно указать 'True' или 'False'.");
                    }
                    administratorState = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public string UserSurname
        {
            get => userSurname;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Фамилия не может быть пустой.");
                    }
                    else
                    {
                        if (value.Length > 2 && value.Length <= 20)
                        {
                            char[] surnameArray = value.ToCharArray();
                            for (int i = 0; i < surnameArray.Length; i++)
                            {
                                if (!char.IsLetter(surnameArray[i]))
                                {
                                    throw new Exception("Вы указали в фамилии недопустимые символы.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Допустимая фамилии имени 3-20 символов.");
                        }
                    }
                    userSurname = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public string UserName
        {
            get => userName;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Имя не может быть пустым.");
                    }
                    else
                    {
                        if (value.Length > 2 && value.Length <= 20)
                        {
                            char[] nameArray = value.ToCharArray();
                            for (int i = 0; i < nameArray.Length; i++)
                            {
                                if (!char.IsLetter(nameArray[i]))
                                {
                                    throw new Exception("Вы указали в имени недопустимые символы.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Допустимая длина имени 3-20 символов.");
                        }
                    }
                    userName = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public User() { }
        public User(string login, string password, string surname, string name)
        {
            UserLogin = login;
            UserPassword = password;
            UserSurname = surname;
            UserName = name;
        }
    }
}
