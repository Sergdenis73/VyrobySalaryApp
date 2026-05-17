using System.Collections.ObjectModel;
using System.Windows;

namespace VyrobySalaryApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<WorkerProduct> workersList;

        private WorkerProduct selectedWorker;

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
                EditGroupBox.Visibility = Visibility.Collapsed;
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

        private void EditDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Authorization.CurrentRole != "Бухгалтер")
            {
                MessageBox.Show("Редагування доступне тільки для Бухгалтера.");
                return;
            }

            EditGroupBox.Visibility = Visibility.Visible;

            MessageBox.Show("Оберіть запис у таблиці подвійним кліком для редагування.");
        }

        private void ProductsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Authorization.CurrentRole != "Бухгалтер")
            {
                return;
            }

            selectedWorker = ProductsDataGrid.SelectedItem as WorkerProduct;

            if (selectedWorker == null)
            {
                MessageBox.Show("Оберіть запис для редагування.");
                return;
            }

            EditGroupBox.Visibility = Visibility.Visible;

            SurnameTextBox.Text = selectedWorker.surname;
            WorkshopTextBox.Text = selectedWorker.workshop_number.ToString();
            ProductATextBox.Text = selectedWorker.product_a.ToString();
            ProductBTextBox.Text = selectedWorker.product_b.ToString();
            ProductCTextBox.Text = selectedWorker.product_c.ToString();
        }

        private void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedWorker == null)
            {
                MessageBox.Show("Спочатку оберіть запис у таблиці подвійним кліком.");
                return;
            }

            int workshopNumber;
            int productA;
            int productB;
            int productC;

            if (SurnameTextBox.Text == "")
            {
                MessageBox.Show("Введіть прізвище складальника.");
                return;
            }

            if (!int.TryParse(WorkshopTextBox.Text, out workshopNumber))
            {
                MessageBox.Show("Номер цеху має бути цілим числом.");
                return;
            }

            if (!int.TryParse(ProductATextBox.Text, out productA))
            {
                MessageBox.Show("Кількість виробів A має бути цілим числом.");
                return;
            }

            if (!int.TryParse(ProductBTextBox.Text, out productB))
            {
                MessageBox.Show("Кількість виробів Б має бути цілим числом.");
                return;
            }

            if (!int.TryParse(ProductCTextBox.Text, out productC))
            {
                MessageBox.Show("Кількість виробів C має бути цілим числом.");
                return;
            }

            selectedWorker.surname = SurnameTextBox.Text;
            selectedWorker.workshop_number = workshopNumber;
            selectedWorker.product_a = productA;
            selectedWorker.product_b = productB;
            selectedWorker.product_c = productC;

            DataAccess dataAccess = new DataAccess();

            bool result = dataAccess.UpdateWorkerProduct(selectedWorker);

            if (result == true)
            {
                MessageBox.Show("Запис успішно відредаговано.");

                LoadDataFromDb();

                EditGroupBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
