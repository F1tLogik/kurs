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
    /// Логика взаимодействия для Staff.xaml
    /// </summary>
    public partial class Staff : Page
    {
        public Staff()
        {
            InitializeComponent();
            UpdatePage();
            //Если пользователь сотрудник
            if (App.currentUser.role == 2)
            {
                ButtonAdd.Visibility = Visibility.Collapsed;
            }
            //Если пользователь администратор
            else
            {
                ButtonAdd.Visibility = Visibility.Visible;
            }
        }

        //Редактирование профиля
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentStaff = (sender as Button).DataContext as Entities.user;
            var role = 2;
            NavigationService.Navigate(new AddClient(currentStaff, role));
        }

        //Удаление сотрудника
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentStaff = (sender as Button).DataContext as Entities.user;
            if (MessageBox.Show($"Вы уверены, что хотите удалить {currentStaff.surname} {currentStaff.firstname} {currentStaff.patronymic}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.users.Remove(currentStaff);
                App.Context.SaveChanges();
                UpdatePage();
            }
        }

        //Добавление сотрудника
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            int role = 2;
            NavigationService.Navigate(new AddClient(role));
        }

        private void UpdatePage()
        {
            // Проверка на подключение к БД
            try
            {
                DataContext = App.Context.users;
                LViewStaff.ItemsSource = App.Context.users.ToList().Where(p => p.role != 1 & p.id_user != App.currentUser.id_user);
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePage();
        }
    }
}
