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
    /// Логика взаимодействия для Treners.xaml
    /// </summary>
    public partial class Treners : Page
    {
        public Treners()
        {
            InitializeComponent();
            UpdatePage();
            //Если пользователь клиент
            if (App.currentUser.role == 1)
            {
                ButtonAdd.Visibility = Visibility.Collapsed;
            }
            //Если пользователь сотрудник
            else
            {
                ButtonAdd.Visibility = Visibility.Visible;
            }
        }

        //Редактирование профиля тренера
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentTrener = (sender as Button).DataContext as Entities.trener;
            NavigationService.Navigate(new AddEditTrener(currentTrener));
        }

        //Удаление профиля тренера
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentTrener = (sender as Button).DataContext as Entities.trener;
            if (MessageBox.Show($"Вы уверены, что хотите удалить {currentTrener.surname} {currentTrener.firstname} {currentTrener.patronymic}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.treners.Remove(currentTrener);
                App.Context.SaveChanges();
                UpdatePage();
            }
        }

        //Добавление нового тренера
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditTrener());
        }
       private void UpdatePage()
        {
            // Проверка на подключение к БД
            try
            {
                DataContext = App.Context.treners;
                LViewTreners.ItemsSource = App.Context.treners.ToList();
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

        //Выбрать тренера для клиента
        private void ButtonSelectTrener_Click(object sender, RoutedEventArgs e)
        {
            var currentTrener = (sender as Button).DataContext as Entities.trener;
            App.currentUser.trener = currentTrener.id_trener;
            App.Context.SaveChanges();
            MessageBox.Show("Тренер выбран");
            NavigationService.GoBack();
        }
    }
}
