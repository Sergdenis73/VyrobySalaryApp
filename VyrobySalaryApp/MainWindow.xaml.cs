using System.Collections.ObjectModel;
using System.Windows;

namespace VyrobySalaryApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<WorkerProduct> workersList;

        public MainWindow()
        {
            InitializeComponent();

            LoadDataFromDb();

            ApplyAccessRights();
        }

        private void LoadDataFromDb()
        {
            DataAccess dataAccess = new DataAccess();

            workersList = dataAccess.LoadWorkersProducts();

            ProductsDataGrid.ItemsSource = workersList;
        }

        private void ApplyAccessRights()
        {
            if (Authorization.CurrentRole == "Бухгалтер")
            {
                AddDataMenuItem.IsEnabled = true;
                EditDataMenuItem.IsEnabled = true;
                StatusTextBlock.Text = "Користувач: Бухгалтер. Повний доступ.";
            }
            else
            {
                AddDataMenuItem.IsEnabled = false;
                EditDataMenuItem.IsEnabled = false;
                StatusTextBlock.Text = "Користувач: Керівник. Режим перегляду.";
            }
        }

        private void AuthMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            bool? result = loginForm.ShowDialog();

            if (result == true)
            {
                ApplyAccessRights();
            }
        }
        private void LoadDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadDataFromDb();

            MessageBox.Show("Дані успішно завантажено з бази даних MySQL.");
        }
    }
}