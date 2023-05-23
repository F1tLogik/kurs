using Microsoft.Win32;
using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using WpfApp1.Entities;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditTrener.xaml
    /// </summary>
    public partial class AddEditTrener : Page
    {
        private Entities.trener currentTrener = null;
        private byte[] mainImageData;
        public AddEditTrener()
        {
            // Проверка на подключение к БД
            try
            {
                InitializeComponent();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\n" +
                    "Проверьте подключение и повторите попытку.\n" +
                    "Приложение будет закрыто.", "Ошибка подключения",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }

        public AddEditTrener(Entities.trener trener)
        {
            // Проверка на подключение к БД
            try
            {
                //Инициализация страницы при редактировании профиля
                InitializeComponent();
                currentTrener = trener;
                Title = "Редактирование профиля";
                TBoxSurname.Text = currentTrener.surname;
                TBoxFirstname.Text = currentTrener.firstname;
                TBoxPatronymic.Text = currentTrener.patronymic;
                TBoxDiscription.Text = currentTrener.discription;
                TBoxExpirience.Text = currentTrener.experience;

                if (currentTrener.trenerImage != null)
                {
                    ImageTrener.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(currentTrener.trenerImage);
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }

        }
    
        //Добавление фотографии
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image | *.png; *.jpg; *.jpeg"
            };
            if (ofd.ShowDialog() == true)
            {
                mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageTrener.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(mainImageData);
            }
        }

        //Сохранение введенной информации
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Добавление информации
                if (currentTrener == null)
                {
                    var trener = new Entities.trener
                    {
                        surname = TBoxSurname.Text,
                        firstname = TBoxFirstname.Text,
                        patronymic = TBoxPatronymic.Text,
                        experience = TBoxExpirience.Text,
                        discription = TBoxDiscription.Text,
                        trenerImage = mainImageData
                    };
                    App.Context.treners.Add(trener);
                    App.Context.SaveChanges();
                    MessageBox.Show("Тренер добавлен");
                }
                //Редактирование информации
                else
                {
                    currentTrener.surname = TBoxSurname.Text;
                    currentTrener.firstname = TBoxFirstname.Text;
                    currentTrener.patronymic = TBoxPatronymic.Text;
                    currentTrener.discription = TBoxDiscription.Text;
                    currentTrener.experience = TBoxExpirience.Text;
                    if (mainImageData != null)
                    {
                        currentTrener.trenerImage = mainImageData;
                    }
                    App.Context.SaveChanges();
                    MessageBox.Show("Информация обновлена");
                }
                NavigationService.GoBack();
            }
        }

        //Проверка ввода данных в поля с добавлением информации
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxFirstname.Text))
            {
                errorBuilder.AppendLine("Имя тренера обязательно для заполнения;");
            }
            if (string.IsNullOrWhiteSpace(TBoxSurname.Text))
            {
                errorBuilder.AppendLine("Фамилия тренера обязательно для заполнения;");
            }
            if (string.IsNullOrWhiteSpace(TBoxPatronymic.Text))
            {
                errorBuilder.AppendLine("Отчество тренера обязательно для заполнения;");
            }
            if (!(Regex.IsMatch(TBoxFirstname.Text, @"[а-я]") || Regex.IsMatch(TBoxFirstname.Text, @"[А-Я]") ||
                Regex.IsMatch(TBoxSurname.Text, @"[а-я]") || Regex.IsMatch(TBoxSurname.Text, @"[А-Я]") ||
                Regex.IsMatch(TBoxPatronymic.Text, @"[а-я]") || Regex.IsMatch(TBoxPatronymic.Text, @"[А-Я]")))
            {
                errorBuilder.AppendLine("Фамилия Имя Отчество должны состоять из русских букв;");
            }
            if (string.IsNullOrWhiteSpace(TBoxExpirience.Text))
            {
                errorBuilder.AppendLine("Опыт работы тренера обязательно для заполнения;");
            }
            if (TBoxExpirience.Text.Length > 20)
            {
                errorBuilder.AppendLine("Опыт работы тренера не должен превышать 20 символов;");
            }
            if (string.IsNullOrWhiteSpace(TBoxDiscription.Text))
            {
                errorBuilder.AppendLine("Описание тренера обязательно для заполнения;");
            }
            if (TBoxDiscription.Text.Length > 350)
            {
                errorBuilder.AppendLine("Описание тренера не должно превышать 250 символов;");
            }
            if (ImageTrener.Source == null)
            {
                errorBuilder.AppendLine("Фото тренера должно присутствовать;");
            }
            if (errorBuilder.Length > 0)
            {
                errorBuilder.Insert(0, "Устраните следующие несоответствия:\n");
            }
            return errorBuilder.ToString();
        }
    }
}
