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
using WpfApp1.Entities;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {
        public Clients()
        {
            InitializeComponent();
            UpdatePage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           UpdatePage();
        }

        private void ButtonAddVisit_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Entities.user;
            var visit = new Entities.visit_log
            {
                id_client = currentClient.id_user,
                date_visit = DateTime.Now
            };
            try
            {
                //Получение информации о покупке абонемента
                var datePurchase = (from sub in App.Context.subscriptions
                                    join subCl in App.Context.subscriptionClients
                                    on sub.id_subscription equals subCl.id_subscription
                                    where subCl.id_client == currentClient.id_user
                                    orderby subCl.id_purchase descending
                                    select subCl.date_purchase).First();
                //Начало действия абонемента
                var timeStart = (from sub in App.Context.subscriptions
                                    join subCl in App.Context.subscriptionClients
                                    on sub.id_subscription equals subCl.id_subscription
                                    where subCl.id_client == currentClient.id_user
                                    orderby subCl.id_purchase descending
                                    select sub.time_start).First();
                //Конец действия абонемента
                var timeEnd = (from sub in App.Context.subscriptions
                                join subCl in App.Context.subscriptionClients
                                on sub.id_subscription equals subCl.id_subscription
                                where subCl.id_client == currentClient.id_user
                                orderby subCl.id_purchase descending
                                select sub.time_end).First();

                var TimeNow = DateTime.Now.TimeOfDay;
                //Проверка на месяц с покупки абонемента
                if (datePurchase.Value.AddMonths(1) < DateTime.Now)
                {
                    MessageBox.Show("Абонемент этого пользователя закончился и совершить посещение сейчас невозможно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                //Проверка на время действия абонемента
                else if (TimeNow >= timeStart && TimeNow <= timeEnd)
                {
                    App.Context.visit_log.Add(visit);
                    App.Context.SaveChanges();
                    MessageBox.Show($"{currentClient.surname} {currentClient.firstname} {currentClient.patronymic} пришел в {DateTime.Now} ");
                }
                else
                {
                    MessageBox.Show("Абонемент этого пользователя не позволяет посетить зал в данный момент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show($"{currentClient.surname} {currentClient.firstname} {currentClient.patronymic} не приобретал(a) абонемент и не имеет возоможности посетить зал");
            }
        }

        //Редактирование клиента
        private void ButtonEditClient_Click(object sender, RoutedEventArgs e)
        {
            var currentStaff = (sender as Button).DataContext as Entities.user;
            var role = 1;
            NavigationService.Navigate(new AddClient(currentStaff, role));
        }

        //Удаление клиента
        private void ButtonDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var currentStaff = (sender as Button).DataContext as Entities.user;
            if (MessageBox.Show($"Вы уверены, что хотите удалить {currentStaff.surname} {currentStaff.firstname} {currentStaff.patronymic}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.users.Remove(currentStaff);
                App.Context.SaveChanges();
                UpdatePage();
            }
        }
        
        //Добавление абонемента
        private void ButtonSelectSubscription__Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Entities.user;
            NavigationService.Navigate(new Subscriptions(currentClient));
        }
        private void UpdatePage()
        {
            // Проверка на подключение к БД
            try
            {
                DataContext = App.Context.users;
                LViewClients.ItemsSource = App.Context.users.ToList().Where(p => p.role == 1);
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }

        //Добавление нового клиента
        private void ButtonAddClient_Click(object sender, RoutedEventArgs e)
        {
            var role = 1;
            NavigationService.Navigate(new AddClient(role));
        }
    }
}
