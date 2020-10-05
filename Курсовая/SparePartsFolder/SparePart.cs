using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace StockRoom
{
    [Table("SparePart")]
    public class SparePart
    {
        private string sparePartN;
        private int sparePartCount;
        private string sparePartCreation;
        private double sparePartCost;
        private string carModelName;
        private string stateName;

        [Key]
        public int SparePartNumber { get; set; }
        public int SparePartCount
        {
            get => sparePartCount;
            set
            {
                try
                {
                    if (Convert.ToInt32(value) >= 0 && Convert.ToInt32(value) <= 100)
                    {
                        char[] countArray = value.ToString().ToCharArray();
                        for (int i = 0; i < countArray.Length; i++)
                        {
                            if (!char.IsDigit(countArray[i]))
                            {
                                throw new Exception("Вы указали в количестве недопустимые символы.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Количество не может быть отрицательным.");
                    }
                    sparePartCount = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public string SparePartCreation
        {
            get => sparePartCreation;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Вы указали неверную дату.");
                    }
                    else
                    {
                        if (!Regex.IsMatch(value, @"([0-2]\d{1}|3[0-1])-(0\d{1}|1[0-2])-(201[7-9]|2020)"))
                        {
                            throw new Exception("Вы ввели неправильную дату. Дата должна выглядеть так: дд-мм-гггг");
                        }
                    }
                    sparePartCreation = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public double SparePartCost
        {
            get => sparePartCost;
            set
            {
                try
                {
                    if (Convert.ToDouble(value) > 0 && Convert.ToDouble(value) <= 13000)
                    {
                        char[] costArray = value.ToString().ToCharArray();
                        for (int i = 0; i < costArray.Length; i++)
                        {
                            if (!char.IsDigit(costArray[i]) && costArray[i] != ',')
                            {
                                throw new Exception("Вы указали в стоимости недопустимые символы.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Введена неверная стоимость. Нужно ввести от 1 до 13000.");
                    }          
                    sparePartCost = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public string CarModelName
        {
            get => carModelName;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Название автобомиля не может быть пустым.");
                    }
                    else
                    {
                        if (value.Length > 2 && value.Length <= 30)
                        {
                            char[] carnameArray = value.ToCharArray();
                            for (int i = 0; i < carnameArray.Length; i++)
                            {
                                if (!char.IsLetter(carnameArray[i]) && carnameArray[i] != ' ')
                                {
                                    throw new Exception("Вы указали в названии недопустимые символы.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Допустимая длина названия автомобиля 3-30 символов.");
                        }
                    }
                    carModelName = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public string SparePartN
        {
            get => sparePartN;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Название запчасти не может быть пустым.");
                    }
                    else
                    {
                        if (value.Length > 2 && value.Length <= 30)
                        {
                            char[] sparePartArray = value.ToCharArray();
                            for (int i = 0; i < sparePartArray.Length; i++)
                            {
                                if (!char.IsLetter(sparePartArray[i]) && sparePartArray[i] != '(' && sparePartArray[i] != ')' && sparePartArray[i] != '-' && sparePartArray[i] != ' ')
                                {
                                    throw new Exception("Вы указали в названии недопустимые символы.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Допустимая длина названия запчасти 3-30 символов.");
                        }
                    }
                    sparePartN = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public string StateName
        {
            get => stateName;
            set
            {
                try
                {
                    if (value == string.Empty)
                    {
                        throw new Exception("Статус не может быть пустым.");
                    }
                    else if (value != "Есть в наличии" && value != "Нет в наличии")
                    {
                        throw new Exception("Допустимое значение 'Есть в наличии' или 'Нет в наличии'");
                    }
                    stateName = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }              
            }
        }

        public SparePart() { }

        public SparePart(int sparePartCount, string sparePartCreation, double sparePartCost, string carModel, string sparePartName, string sparePartStatus)
        {
            SparePartCount = sparePartCount;
            SparePartCreation = sparePartCreation;
            SparePartCost = sparePartCost;
            CarModelName = carModel;
            SparePartN = sparePartName;
            StateName = sparePartStatus;
        }
    }
}
