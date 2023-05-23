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
    /// Логика взаимодействия для Visits.xaml
    /// </summary>
    public partial class Visits : Page
    {
        public Visits(user client)
        {
            // Проверка на подключение к БД
            try
            {
                InitializeComponent();
                DataContext = App.Context.visit_log;
                //Выборка посещений по текущему клиенту
                LViewVisits.ItemsSource = App.Context.visit_log.Where(v => v.id_client == client.id_user).ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }
    }
}
