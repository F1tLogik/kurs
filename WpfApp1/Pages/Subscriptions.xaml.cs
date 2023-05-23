using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Entities;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для Subscriptions.xaml
    /// </summary>
    public partial class Subscriptions : Page
    {
        private Entities.user currentClient = null;
        public Subscriptions()
        {
            // Проверка на подключение к БД
            try
            {
                InitializeComponent();
                DataContext = App.Context.subscriptions;
                LViewSubscription.ItemsSource = App.Context.subscriptions.ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }
        public Subscriptions(Entities.user client)
        {
            // Проверка на подключение к БД
            try
            {
                InitializeComponent();
                DataContext = App.Context.subscriptions;
                LViewSubscription.ItemsSource = App.Context.subscriptions.ToList();
                currentClient = client;
            }
            catch (Exception)
            {

                throw;
            }
        }

       //Добавление нового абонемента
        private void ButtonSelectSubscription_Click(object sender, RoutedEventArgs e)
        {
            
            var currentSubscription = (sender as Button).DataContext as Entities.subscription;
            var purchase = new Entities.subscriptionClient
            {
                id_client = currentClient.id_user,
                id_subscription = currentSubscription.id_subscription,
                date_purchase = DateTime.Now
            };
           

            DialogResult result = MessageBox.Show($"Уверены, что хотите подключить {currentSubscription.discription_subscription}","Подтверждение", (MessageBoxButtons)MessageBoxButton.YesNo, (MessageBoxIcon)MessageBoxImage.Question);

            if (result == DialogResult.Yes)
            {
                App.Context.subscriptionClients.Add(purchase);
                App.Context.SaveChanges();
                MessageBox.Show("Абонемент активирован");
                NavigationService.GoBack();
            }
            
        }
    }
}
