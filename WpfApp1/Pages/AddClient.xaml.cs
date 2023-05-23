 using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Page
    {
        private Entities.user currentClient = null;
        private byte[] mainImageData;
        private int userRole;
        public AddClient(int role)
        {
            // Проверка на подключение к БД
            try
            {
                userRole = role;
                InitializeComponent();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
            
        }

        public AddClient(Entities.user client, int role)
        {
            // Проверка на подключение к БД
            try
            {
                //Инициализация страницы при редактировании профиля
                InitializeComponent();
                Title = "Редактирование пользователя";
                currentClient = client;
                userRole = role;
                TBoxFirstname.Text = currentClient.firstname;
                TBoxSurname.Text = currentClient.surname;
                TBoxPatronymic.Text = currentClient.patronymic;
                TBoxPhoneNumber.Text = Convert.ToString(currentClient.phone_number);
                if (client.role == 1)
                {
                    TBoxPassword.Text = currentClient.password;
                }
                else
                {
                    TBlockPassword.Visibility = Visibility.Collapsed;
                    TBoxPassword.Visibility = Visibility.Collapsed;
                }

                if (currentClient.userImage != null)
                {
                    ImageClient.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(currentClient.userImage);
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }


        //Проверка ввода данных в поля с добавлением информации
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxFirstname.Text) ||
                string.IsNullOrWhiteSpace(TBoxSurname.Text) ||
                string.IsNullOrWhiteSpace(TBoxPatronymic.Text))
            {
                errorBuilder.AppendLine("ФИО клиента должно быть заполнено;");
            }
            if (TBoxFirstname.Text.Length > 25 || TBoxPatronymic.Text.Length > 25 || TBoxSurname.Text.Length > 25)
            {
                errorBuilder.AppendLine("Фамилия Имя Отчество должны быть короче 25 символов;");
            }
            if (!(Regex.IsMatch(TBoxFirstname.Text, @"[а-я]") || Regex.IsMatch(TBoxFirstname.Text, @"[А-Я]") ||
                Regex.IsMatch(TBoxSurname.Text, @"[а-я]") || Regex.IsMatch(TBoxSurname.Text, @"[А-Я]") ||
                Regex.IsMatch(TBoxPatronymic.Text, @"[а-я]") || Regex.IsMatch(TBoxPatronymic.Text, @"[А-Я]")))
            {
                errorBuilder.AppendLine("Фамилия Имя Отчество должны состоять из русских букв;");
            }
            var phone_numberFromDB = App.Context.users.ToList().FirstOrDefault(p => p.phone_number == TBoxPhoneNumber.Text);
            if (phone_numberFromDB != null && phone_numberFromDB != currentClient)
            {
                errorBuilder.AppendLine("Пользователь с таким номером телефона уже есть;");
            }
            if(TBoxPhoneNumber.Text.Length != 11)
            {
                errorBuilder.AppendLine("Номер телефона состоит из 11 цифр;");
            }
            if(Regex.IsMatch(TBoxPhoneNumber.Text, @"\D"))
            {
                errorBuilder.AppendLine("Номер телефона состоит только из цифр");
            }
            if (ImageClient.Source == null)
            {
                errorBuilder.AppendLine("Фото пользователя должно присутствовать;");
            }
            if(TBoxPassword.Text.Length > 25)
            {
                errorBuilder.AppendLine("Пароль не должен превышать 25 символов;");
            }
            if(errorBuilder.Length > 0)
            {
                errorBuilder.Insert(0, "Устраните следующие несоответствия:\n");
            }

            return errorBuilder.ToString();
        }


        //Сохранение введенной информации
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (currentClient == null)
                {
                    if (userRole == 1)
                    {
                        var client = new Entities.user
                        {
                            surname = TBoxSurname.Text,
                            firstname = TBoxFirstname.Text,
                            patronymic = TBoxPatronymic.Text,
                            phone_number = TBoxPhoneNumber.Text,
                            password = TBoxPassword.Text,
                            role = 1,
                            userImage = mainImageData
                        };
                        App.Context.users.Add(client);
                        App.Context.SaveChanges();
                        MessageBox.Show("Клиент добавлен");
                    }
                    else
                    {
                        var stuff = new Entities.user
                        {
                            surname = TBoxSurname.Text,
                            firstname = TBoxFirstname.Text,
                            patronymic = TBoxPatronymic.Text,
                            phone_number = TBoxPhoneNumber.Text,
                            role = 2,
                            userImage = mainImageData
                        };
                        App.Context.users.Add(stuff);
                        App.Context.SaveChanges();
                        MessageBox.Show("Сотрудник добавлен");
                    }
                }
                else
                {
                    currentClient.firstname = TBoxFirstname.Text;
                    currentClient.surname = TBoxSurname.Text;
                    currentClient.patronymic = TBoxPatronymic.Text;
                    currentClient.phone_number = TBoxPhoneNumber.Text;
                    if (mainImageData != null)
                    {
                        currentClient.userImage = mainImageData;
                    }
                    App.Context.SaveChanges();
                    MessageBox.Show("Информация обновлена");
                }
                NavigationService.GoBack();
            }
        }

        //Выбор фотографии
        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image | *.png; *.jpg; *.jpeg"
            };
            if (ofd.ShowDialog() == true)
            {
                mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageClient.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(mainImageData);
            }
        }
    }
}
