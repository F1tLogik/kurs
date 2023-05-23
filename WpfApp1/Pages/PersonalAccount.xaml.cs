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
using WpfApp1.Entities;


namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccount.xaml
    /// </summary>
    public partial class PersonalAccount : Page
    { 
        public PersonalAccount()
        {
            InitializeComponent();
            DataContext = App.currentUser;
            UpdatePage();
            try
            {
                //Получение название его абонемента
                var nameSubscription = from sub in App.Context.subscriptions
                                            join subCl in App.Context.subscriptionClients
                                            on sub.id_subscription equals subCl.id_subscription
                                            where subCl.id_client == App.currentUser.id_user
                                            orderby subCl.id_purchase descending
                                            select sub.discription_subscription;
                NameSubscription.Text = nameSubscription.First();

                //Получение даты покупки абонемента
                var datePurchase = (from sub in App.Context.subscriptions
                                    join subCl in App.Context.subscriptionClients
                                    on sub.id_subscription equals subCl.id_subscription
                                    where subCl.id_client == App.currentUser.id_user
                                    orderby subCl.date_purchase descending
                                    select subCl.date_purchase).First();

                //Проверка на месяц с покупки абонемента
                if (datePurchase.Value.AddMonths(1) < DateTime.Now)
                {
                    TBDateAccess.Text = "Ваш абонемент закончился";
                    TBDateAccess.Foreground = Brushes.Red;
                }
                else
                {
                    TBDateAccess.Text = $"Активен до {Convert.ToString(datePurchase.Value.AddMonths(1).ToString("dd.MM.yyyy"))}";
                }
            }
            catch
            {
                SubscriptionPanel.Visibility = Visibility.Collapsed;
            }
            
        }

        //Редактирование профиля
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Entities.user;
            NavigationService.Navigate(new EditPersonAccount(currentClient));

        }

        //Переход к списку посещений
        private void ButtonVisitLog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Visits(App.currentUser));
        }

        //Переход к списку тренеров
        private void ButtonTreners_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Treners());
        }

        //Переход к списку абонементов
        private void ButtonSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Subscriptions());
        }
         private void UpdatePage()
        {
            // Проверка на подключение к БД
            try
            {
                //Получение информации о росте и весе пользователя
                double userWeight = Convert.ToDouble(App.currentUser.weight);
                double userHeight = Convert.ToDouble(App.currentUser.height);
                DataContext = App.currentUser;
                //Если есть информации о весе и росте пользователя
                if (App.currentUser.height != null && App.currentUser.weight != null || CalculationClass.CalculationIMT(userWeight, userHeight) != 0)
                {
                    TBlockHeight.Text = Convert.ToString(App.currentUser.height);
                    TBlockWeight.Text = Convert.ToString(App.currentUser.weight);
                    TextBoxIMT.Text = Convert.ToString(CalculationClass.CalculationIMT(userWeight, userHeight));
                    //Соответствие цвета к уровню ИМТ
                    if (CalculationClass.CalculationIMT(userWeight, userHeight) >= 18.5 &&
                        CalculationClass.CalculationIMT(userWeight, userHeight) < 25)
                    {
                        TextBoxIMT.Foreground = Brushes.Green;
                    }
                    else if (CalculationClass.CalculationIMT(userWeight, userHeight) >= 25 &&
                             CalculationClass.CalculationIMT(userWeight, userHeight) < 30 ||
                             CalculationClass.CalculationIMT(userWeight, userHeight) >= 16 &&
                             CalculationClass.CalculationIMT(userWeight, userHeight) < 18.5)
                    {
                        TextBoxIMT.Foreground = Brushes.Orange;
                    }
                    else
                    {
                        TextBoxIMT.Foreground = Brushes.Red;
                    }
                }
                //Если информации о весе и росте человека нету
                else
                {
                    PersonHeight.Visibility = Visibility.Collapsed;
                    PersonWeight.Visibility = Visibility.Collapsed;
                    PersonIMT.Visibility = Visibility.Collapsed;
                }

                //Если текущего тренера у клиента нет
                if (App.currentUser.trener == null)
                {
                    TrenerPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TrenerPanel.Visibility = Visibility.Visible;
                    //Получение фамилии тренера клиента
                    var surnameTrener = (from tr in App.Context.treners
                                         join us in App.Context.users
                                         on tr.id_trener equals us.trener
                                         where App.currentUser.trener == tr.id_trener
                                         select tr.surname).First();
                    //Получение имени тренера клиента
                    var firstnameTrener = (from tr in App.Context.treners
                                           join us in App.Context.users
                                           on tr.id_trener equals us.trener
                                           where App.currentUser.trener == tr.id_trener
                                           select tr.firstname).First();
                    //Получение отчества тренера клиента
                    var patronymicTrener = (from tr in App.Context.treners
                                            join us in App.Context.users
                                            on tr.id_trener equals us.trener
                                            where App.currentUser.trener == tr.id_trener
                                            select tr.patronymic).First();
                    
                    TBlockTrenerSurname.Text = $"{surnameTrener}";
                    TBlockTrenerFirstname.Text = $"{firstnameTrener}";
                    TBlockTrenerPatronymic.Text = $"{patronymicTrener}";
                }
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

        //Переход к списку личных достижений
        private void ButtonPersonAchievements_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListAcheivement());
        }
    }
}
