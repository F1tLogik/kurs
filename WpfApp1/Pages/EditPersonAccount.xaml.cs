using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Entities;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPersonAccount.xaml
    /// </summary>
    public partial class EditPersonAccount : Page
    {
        private Entities.user currentClient = null;
        public EditPersonAccount()
        {
            // Проверка на подключение к БД
            try
            {
                InitializeComponent();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }

        public EditPersonAccount(Entities.user client)
        {
            // Проверка на подключение к БД
            try
            {
                //Инициализация страницы при редактировании профиля
                InitializeComponent();
                currentClient = client;
                if (client.role == 1)
                {
                    TBoxHeight.Text = Convert.ToString(currentClient.height);
                    TBoxWeight.Text = Convert.ToString(currentClient.weight);
                }
                else
                {
                    Title = "Смена пароля";
                    WeightHeight.Visibility = Visibility.Hidden;
                }
                if (Regex.IsMatch(TBoxWeight.Text, @"[,]"))
                {
                    TBoxWeight.MaxLength = 6;
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }

        //Сохранение информации
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                //Если текущий пользователь клиент
                if (currentClient.role == 1)
                {
                    if (PBoxOld.Password == "" && PBoxNew.Password == "" && PBoxNewRepeat.Password == "")
                    {
                        if (TBoxHeight.Text == null || TBoxHeight.Text == null)
                        {
                            currentClient.height = null;
                        }
                        else if (TBoxWeight.Text == null || TBoxWeight.Text == "")
                        {
                            currentClient.weight = null;
                        }
                        else
                        {
                            currentClient.weight = Convert.ToDouble(TBoxWeight.Text);
                            currentClient.height = Convert.ToInt32(TBoxHeight.Text);
                            App.Context.SaveChanges();
                            MessageBox.Show("Информация обновлена");
                            NavigationService.GoBack();
                        }
                    }
                    else
                    {
                        if (PBoxNew.Password == "" || PBoxNewRepeat.Password == "")
                        {
                            MessageBox.Show("Пароли не введены");
                        }
                        else if (PBoxNew.Password != PBoxNewRepeat.Password)
                        {
                            MessageBox.Show("Пароли не совпадают");
                        }
                        else if (Convert.ToString(currentClient.password) != PBoxOld.Password)
                        {
                            MessageBox.Show("Старый пароль неверен");
                        }
                        else if (PBoxOld.Password == currentClient.password && PBoxNew.Password == PBoxNewRepeat.Password)
                        {
                            currentClient.weight = Convert.ToDouble(TBoxWeight.Text);
                            currentClient.height = Convert.ToInt32(TBoxHeight.Text);
                            currentClient.password = PBoxNew.Password;
                            App.Context.SaveChanges();
                            MessageBox.Show("Информация обновлена");
                            NavigationService.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Неопознанная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                //Если текущий пользователь сотрудник
                else
                {
                    if (PBoxOld.Password == "" && PBoxNew.Password == "" && PBoxNewRepeat.Password == "")
                    {
                        MessageBox.Show("Поля не заполнены");
                    }
                    else if (PBoxNew.Password == "" || PBoxNewRepeat.Password == "")
                    {
                        MessageBox.Show("Пароли не введены");
                    }
                    else if (PBoxNew.Password != PBoxNewRepeat.Password)
                    {
                        MessageBox.Show("Пароли не совпадают");
                    }
                    else if (PBoxOld.Password == App.currentUser.password && PBoxNew.Password == PBoxNewRepeat.Password)
                    {
                        currentClient.password = PBoxNew.Password;
                        App.Context.SaveChanges();
                        MessageBox.Show("Информация обновлена");
                        NavigationService.GoBack();
                    }
                    else if (App.currentUser.password != PBoxOld.Password)
                    {
                        MessageBox.Show("Старый пароль не верен");
                    }
                    else
                    {
                        MessageBox.Show("Неопознанная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        //Проверка ввода данных в поля с добавлением информации

        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();
            char ch = Convert.ToChar(",");
            int count = 0;
            foreach (char c in TBoxWeight.Text)
            {
                if (c == ch)
                {
                    count++;
                }
            }
            if (count >= 2)
            {
                errorBuilder.AppendLine("Формат числа нарушен");
            }
            if (Regex.IsMatch(TBoxHeight.Text, @"\D"))
            {
                errorBuilder.AppendLine("Рост вводится целыми числами");
            }
            if (PBoxNew.Password.Length > 25)
            {
                errorBuilder.AppendLine("Пароль должен быть меньше 25 символов");
            }
            if (errorBuilder.Length > 0)
            {
                errorBuilder.Insert(0, "Устраните следующие несоответствия:\n");
            }
            

            return errorBuilder.ToString();
        }

        //Изменение цвета в соответствии с его уровнем
        private void PBoxNew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            TBlockLvlPass.Text = CalculationClass.PasswordCheck(PBoxNew.Password);
            if (TBlockLvlPass.Text == "Слабый пароль")
                TBlockLvlPass.Foreground = Brushes.Red;

            else if (TBlockLvlPass.Text == "Простой пароль")
                TBlockLvlPass.Foreground = Brushes.Orange;

            else if (TBlockLvlPass.Text == "Хороший пароль")
                TBlockLvlPass.Foreground = Brushes.Blue;

            else if (TBlockLvlPass.Text == "Сложный пароль")
                TBlockLvlPass.Foreground = Brushes.DarkGreen;

            else if (TBlockLvlPass.Text == "Очень cложный пароль")
                TBlockLvlPass.Foreground = Brushes.DarkOliveGreen;

            else if (TBlockLvlPass.Text == "Ты такой запомнишь вообще?")
                TBlockLvlPass.Foreground = Brushes.DarkMagenta;

            else
                TBlockLvlPass.Foreground = Brushes.Black;

        }

        private void TBoxWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TBoxWeight.Text == null || TBoxWeight.Text == "")
            {
                TBoxWeight.Text = "0";
            }
            if (TBoxWeight.Text[0] == '1')
            {
                TBoxWeight.MaxLength = 3;
            }
            else
            {
                TBoxWeight.MaxLength = 2;
            }
            if (sender is TextBox textBox)
            {
                    textBox.Text = new string
                        (textBox.Text.Where(ch => (ch >= '0' && ch <= '9')).ToArray());
            }
        }

        private void TBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = new string
                    (textBox.Text.Where(ch => (ch >= '0' && ch <= '9')).ToArray());
            }
            if (TBoxHeight.Text == null || TBoxHeight.Text == "")
            {
                TBoxHeight.Text = "0";
            }
            if (TBoxHeight.Text[0] == '1' || TBoxHeight.Text[0] == '2')
            {
                TBoxHeight.MaxLength = 3;
            }
            else
            {
                TBoxHeight.MaxLength = 2;
            }
        }
    }
}
