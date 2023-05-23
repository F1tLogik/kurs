using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace WpfApp1.Entities
{
    public class CalculationClass
    {
        //Установка уровня пароля
        public static string PasswordCheck(string password)
        {
            int point = 0;
            if (password.Length < 6)
            {
                point --;
            }
            if (password.Length > 8)
                point ++;
            if (password.Length > 16)
                point ++;

            if (Regex.IsMatch(password, @"\W"))
                point ++;

            if (Regex.IsMatch(password, @"\d"))
                point ++;

            if (Regex.IsMatch(password, @"[а-я]"))
                point ++;

            if (Regex.IsMatch(password, @"[А-Я]"))
                point ++;

            if (Regex.IsMatch(password, @"[a-z]"))
                point++;

            if (Regex.IsMatch(password, @"[A-Z]"))
                point++;

            if (password.Length == 0)
            {
                return "";
            }
            else if (point == 0 || point == 1)
            {
                return "Слабый пароль";
            }
            else if (point == 2 || point == 3)
            {
                return "Простой пароль";
            }

            else if (point == 4)
            {
                return "Хороший пароль";
            }

            else if (point == 5)
            {
                return "Сложный пароль";
            }
            else if (point == 6)
            {
                return "Очень сложный пароль";
            }
            else if (point == 7)
            {
                return "Ты такой запомнишь вообще?";
            }
            else
                return "Нуу ладно...";
        }

        //Вычисление показателя ИМТ пользователя
        public static double CalculationIMT(double weight, double height)
        {
            double imt;
            if (weight <= 0 || height <= 0)
            {
                imt = 0;
            }
            else
            {
                height /= 100;
                imt = Math.Round(weight / (Math.Pow((height), 2)), 2);
            }
            return imt;
        }
    }
}
