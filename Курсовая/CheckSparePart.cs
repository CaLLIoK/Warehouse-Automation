using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace StockRoom
{
    static class CheckSparePart
    {
        public static string CheckSparePartName(string str)
        {
            if (str == string.Empty)
            {
                return "Название запчасти не может быть пустым.";
            }
            else
            {
                if (str.Length > 2 && str.Length <= 30)
                {
                    char[] sparePartArray = str.ToCharArray();
                    for (int i = 0; i < sparePartArray.Length; i++)
                    {
                        if (!char.IsLetter(sparePartArray[i]) && sparePartArray[i] != '(' && sparePartArray[i] != ')' && sparePartArray[i] != '-' && sparePartArray[i] != ' ')
                        {
                            return "Вы указали в названии запчасти недопустимые символы.";
                        }
                    }
                }
                else
                {
                    return "Допустимая длина названия запчасти 3-30 символов.";
                }
            }
            return str;
        }

        public static string CheckCarName (string str)
        {
            if (str == string.Empty)
            {
                return "Название автомобиля не может быть пустым.";
            }
            else
            {
                if (str.Length > 2 && str.Length <= 30)
                {
                    char[] carNameArray = str.ToCharArray();
                    for (int i = 0; i < carNameArray.Length; i++)
                    {
                        if (!char.IsLetter(carNameArray[i]) && carNameArray[i] != ' ')
                        {
                            return "Вы указали в названии автомобиля недопустимые символы.";
                        }
                    }
                }
                else
                {
                    return "Допустимая длина названия автомобиля 3-30 символов.";
                }
            }
            return str;
        }

        public static string CheckCreationDate (string str)
        {
            if (str == string.Empty)
            {
                return "Вы не указали дату.";
            }
            else
            {
                DateTime fromDateValue;
                var formats = new[] { "dd-MM-yyyy" };
                if (!DateTime.TryParseExact(str, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateValue))
                {
                    return "Вы ввели неправильную дату. \nДата должна выглядеть так: дд-мм-гггг, \nгде дд(01-31), \n       мм(01-12), \n       гггг(2017-2020) ";
                }
            }
            return str;
        }

        public static string CheckSparePartCost (string str)
        {
            if (str == string.Empty)
            {
                return "Стоимость не может быть пустой.";
            }
            else
            {
                char[] costArray = str.ToCharArray();
                for (int i = 0; i < costArray.Length; i++)
                {
                    if ((!char.IsDigit(costArray[i]) && costArray[i] != ','))
                    {
                        return "Вы указали в стоимости недопустимые символы.";
                    }
                }
                if (Convert.ToDouble(str) < 0 || Convert.ToDouble(str) > 13000)
                {
                    return "Введена неверная стоимость. Нужно ввести от 1 до 10000.";
                }
            }
            return str;
        }

        public static string CheckSparePartCount (string str)
        {
            if (str == string.Empty)
            {
                return "Количество не может быть пустым.";
            }
            else
            {
                char[] countArray = str.ToCharArray();
                for (int i = 0; i < countArray.Length; i++)
                {
                    if (!char.IsDigit(countArray[i]))
                    {
                        return "Вы указали в количестве недопустимые символы.";
                    }
                }
                if (Convert.ToInt32(str) < 0)
                {
                    return "Кооличество не может быть отрицательным.";
                }
            }
            return str;
        }
    }
}