using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    /// Логика взаимодействия для ListAcheivement.xaml
    /// </summary>
    public partial class ListAcheivement : Page
    {
        public ListAcheivement()
        {
            InitializeComponent();
            UpdatePage();
        }

        //Редактирование упражнения
        private void ButtonEditExerceise_Click(object sender, RoutedEventArgs e)
        {
            var currentExrcise = (sender as Button).DataContext as Entities.list_of_exercise;
            NavigationService.Navigate(new AddEditAchievement(currentExrcise));
        }

        //Удаление упражнения
        private void ButtonDeleteExercise_Click(object sender, RoutedEventArgs e)
        {
            var currentExrcise = (sender as Button).DataContext as Entities.list_of_exercise;
            if (MessageBox.Show($"Вы уверены что хотите удалить упражнение - \"{currentExrcise.name_exercise}\"", "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                App.Context.list_of_exercise.Remove(currentExrcise);
                App.Context.SaveChanges();
                UpdatePage();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePage();
        }
        private void UpdatePage()
        {
            // Проверка на подключение к БД
            try
            {
                DataContext = App.Context.list_of_exercise;
                LViewAchievement.ItemsSource = App.Context.list_of_exercise.Where(p => p.id_client == App.currentUser.id_user).ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }

        //Добавление нового упражнения
        private void ButtonAddExercise_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditAchievement());
        }

        //Метод создания PDF файла
        public static void Print(Visual elementToPrint, string description)
        {
            using(var printServer = new LocalPrintServer())
            {
                var dialog = new PrintDialog();
                dialog.ShowDialog();
                var qs = printServer.GetPrintQueues();
                dialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
                dialog.PrintVisual(elementToPrint, description);
            }
        }

        //Создание PDF файла
        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            ButtonPrint.Visibility = Visibility.Collapsed;
            ButtonAddExercise.Visibility = Visibility.Collapsed;
            Print(this, Convert.ToString("Упражнения"));
            ButtonAddExercise.Visibility = Visibility.Visible;
            ButtonPrint.Visibility = Visibility.Visible;
        }
    }
}
