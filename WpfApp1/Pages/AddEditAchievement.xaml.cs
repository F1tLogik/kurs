using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditAchievement.xaml
    /// </summary>
    public partial class AddEditAchievement : Page
    {
        private Entities.list_of_exercise currentExercise = null;
        public AddEditAchievement()
        {
            //Проверка на подключение к БД
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
        public AddEditAchievement(Entities.list_of_exercise exercise)
        {
            //Проверка на подключение к БД
            try
            {
                //Инициализация страницы при редактировании упражнения
                InitializeComponent();
                currentExercise = exercise;
                Title = "Редактирование";
                TBoxAmountRepetition.Text = Convert.ToString(currentExercise.repetition);
                TBoxNameExercise.Text = currentExercise.name_exercise;
                TBoxWeight.Text = Convert.ToString(currentExercise.weight);
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBox.Show("Отсутствует подключение к серверу.\nПроверьте подключение и повторите попытку.\nПриложение будет закрыто.", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Windows[0].Close();
            }
        }

        //Сохранение упражнения
        private void ButtonSaveAchievement_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Добавление упражнения
                if (currentExercise == null)
                {
                    var exercise = new Entities.list_of_exercise
                    {
                        name_exercise = TBoxNameExercise.Text,
                        repetition = Convert.ToInt32(TBoxAmountRepetition.Text),
                        weight = Convert.ToInt32(TBoxWeight.Text),
                        id_client = App.currentUser.id_user
                    };
                    App.Context.list_of_exercise.Add(exercise);
                    App.Context.SaveChanges();
                    MessageBox.Show("Упражнение добавлено");
                }
                //Редактирование упражнения
                else
                {
                    currentExercise.name_exercise = TBoxNameExercise.Text;
                    currentExercise.repetition = Convert.ToInt32(TBoxAmountRepetition.Text);
                    currentExercise.weight = Convert.ToInt32(TBoxWeight.Text);
                    App.Context.SaveChanges();
                    MessageBox.Show("Информация обновлена");
                }
                NavigationService.GoBack();
            }
        }

        //Проверка ввода данных в поля с добавлением информации
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TBoxAmountRepetition.Text) ||
                string.IsNullOrWhiteSpace(TBoxNameExercise.Text) ||
                string.IsNullOrWhiteSpace(TBoxWeight.Text))
            {
                errorBuilder.AppendLine("Все поля обязательны для заполнения;");
            }
            if (Regex.IsMatch(TBoxAmountRepetition.Text, @"\D"))
            {
                errorBuilder.AppendLine("Количество повторений должно указываться в целых числах;");
            }
            if (Regex.IsMatch(TBoxWeight.Text, @"\D"))
            {
                errorBuilder.AppendLine("Используемый вес должен указываться в целых числах;");
            }
            if (TBoxNameExercise.Text.Length > 50)
            {
                errorBuilder.AppendLine("Количество символов для ввода названия упражнения ограничено 50 символами");
            }
            if (TBoxWeight.Text.Length > 4)
            {
                errorBuilder.AppendLine("Количество символов для ввода веса ограничено 3 символами");
            }
            if (TBoxAmountRepetition.Text.Length > 4)
            {
                errorBuilder.AppendLine("Количество символов для ввода количества повторений ограничено 3 символами");
            }
            return errorBuilder.ToString();
        }
    }
}
