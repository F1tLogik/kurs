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
    /// Логика взаимодействия для MenuStaff.xaml
    /// </summary>
    public partial class MenuStaff : Page
    {
        public MenuStaff()
        {
            // Проверка на подключение к БД
            try
            {
                InitializeComponent();
                DataContext = App.currentUser; 
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }
        //Переход к списку клиентов
        private void ButtonClients_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Clients());
        }

        //Переход к списку тренеров
        private void ButtonTreners_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Treners());
        }

        //Переход к списку сотрудников
        private void ButtonStaff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Staff());
        }

        //Изменение пароля
        private void ButtonEditPassword_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Entities.user;
            NavigationService.Navigate(new EditPersonAccount(currentClient));
        }
    }
}
