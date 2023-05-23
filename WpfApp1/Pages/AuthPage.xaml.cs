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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
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

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на подключение к БД
            try
            {
                //Авторизация
                var currentClient = App.Context.users.FirstOrDefault(p => p.phone_number == TBoxNP.Text && p.password == PBoxPassword.Password);
                if (PBoxPassword.Password == null || PBoxPassword.Password == "")
                {
                    MessageBox.Show("Пароль не введен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else if (currentClient != null)
                {
                    if (currentClient.role == 1)
                    {
                        App.currentUser = currentClient;
                        NavigationService.Navigate(new PersonalAccount());
                    }
                    else
                    {
                        App.currentUser = currentClient;
                        NavigationService.Navigate(new MenuStaff());
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }

        }
    }
}
