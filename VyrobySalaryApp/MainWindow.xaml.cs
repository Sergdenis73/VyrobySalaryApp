using System.Collections.ObjectModel;
using System.Windows;

namespace VyrobySalaryApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<WorkerProduct> workersList;

        private WorkerProduct selectedWorker;

        private string searchMode = "";

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
                DeleteDataMenuItem.IsEnabled = true; // Дозволяємо видаляти
                StatusTextBlock.Text = "Користувач: Бухгалтер. Повний доступ.";
            }
            else
            {
                AddDataMenuItem.IsEnabled = false;
                EditDataMenuItem.IsEnabled = false;
                DeleteDataMenuItem.IsEnabled = false; // Блокуємо видалення
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

            MessageBox.Show("Оберіть запис у таблиці подвійним кліком для редагування.");
        }

        private void AddDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Authorization.CurrentRole != "Бухгалтер")
            {
                MessageBox.Show("Додавання доступне тільки для Бухгалтера.");
                return;
            }

            // Очищаємо всі поля
            SurnameTextBox.Text = "";
            WorkshopTextBox.Text = "";
            ProductATextBox.Text = "";
            ProductBTextBox.Text = "";
            ProductCTextBox.Text = "";

            // Показуємо панель, підміняємо кнопки
            EditGroupBox.Visibility = Visibility.Visible;
            SaveEditButton.Visibility = Visibility.Collapsed;
            SaveNewButton.Visibility = Visibility.Visible;
        }

        private void DeleteDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Authorization.CurrentRole != "Бухгалтер")
            {
                MessageBox.Show("Видалення доступне тільки для Бухгалтера.");
                return;
            }

            WorkerProduct toDelete = ProductsDataGrid.SelectedItem as WorkerProduct;

            if (toDelete == null)
            {
                MessageBox.Show("Оберіть запис у таблиці, який бажаєте видалити.");
                return;
            }

            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"Ви дійсно хочете видалити запис складальника {toDelete.surname}?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DataAccess dataAccess = new DataAccess();
                bool result = dataAccess.DeleteWorkerProduct(toDelete.id);

                if (result)
                {
                    MessageBox.Show("Запис успішно видалено.");
                    LoadDataFromDb();
                    EditGroupBox.Visibility = Visibility.Collapsed;
                }
            }
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

            // Показуємо панель, підміняємо кнопки навпаки (показуємо Зберегти старе)
            EditGroupBox.Visibility = Visibility.Visible;
            SaveEditButton.Visibility = Visibility.Visible;
            SaveNewButton.Visibility = Visibility.Collapsed;

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

        private void SaveNewButton_Click(object sender, RoutedEventArgs e)
        {
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

            WorkerProduct newWorker = new WorkerProduct
            {
                surname = SurnameTextBox.Text,
                workshop_number = workshopNumber,
                product_a = productA,
                product_b = productB,
                product_c = productC
            };

            DataAccess dataAccess = new DataAccess();
            bool result = dataAccess.InsertWorkerProduct(newWorker);

            if (result == true)
            {
                MessageBox.Show("Новий запис успішно додано до бази даних.");
                LoadDataFromDb();
                EditGroupBox.Visibility = Visibility.Collapsed;
            }
        }

        private void SelectWorkshopMenuItem_Click(object sender, RoutedEventArgs e)
        {
            searchMode = "X";

            SearchGroupBox.Visibility = Visibility.Visible;

            SearchSurnameLabel.Visibility = Visibility.Collapsed;
            SearchSurnameTextBox.Visibility = Visibility.Collapsed;

            SearchWorkshopTextBox.Text = "";
            SearchSurnameTextBox.Text = "";

            MessageBox.Show("Введіть номер цеху для відбору записів.");
        }

        private void SelectWorkshopSurnameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            searchMode = "XY";

            SearchGroupBox.Visibility = Visibility.Visible;

            SearchSurnameLabel.Visibility = Visibility.Visible;
            SearchSurnameTextBox.Visibility = Visibility.Visible;

            SearchWorkshopTextBox.Text = "";
            SearchSurnameTextBox.Text = "";

            MessageBox.Show("Введіть номер цеху та прізвище складальника для відбору записів.");
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            int workshopNumber;

            if (SearchWorkshopTextBox.Text == "")
            {
                MessageBox.Show("Введіть номер цеху.");
                return;
            }

            if (!int.TryParse(SearchWorkshopTextBox.Text, out workshopNumber))
            {
                MessageBox.Show("Номер цеху має бути цілим числом.");
                return;
            }

            if (workshopNumber <= 0)
            {
                MessageBox.Show("Номер цеху має бути додатним числом.");
                return;
            }

            DataAccess dataAccess = new DataAccess();

            if (searchMode == "X")
            {
                workersList = dataAccess.SelectByWorkshop(workshopNumber);
            }
            else if (searchMode == "XY")
            {
                string surname = SearchSurnameTextBox.Text;

                if (surname == "")
                {
                    MessageBox.Show("Введіть прізвище складальника.");
                    return;
                }

                bool hasDigit = false;

                for (int i = 0; i < surname.Length; i++)
                {
                    if (char.IsDigit(surname[i]))
                    {
                        hasDigit = true;
                    }
                }

                if (hasDigit == true)
                {
                    MessageBox.Show("Прізвище повинно містити літери.");
                    return;
                }

                workersList = dataAccess.SelectByWorkshopAndSurname(workshopNumber, surname);
            }
            else
            {
                MessageBox.Show("Оберіть режим пошуку.");
                return;
            }

            ProductsDataGrid.ItemsSource = workersList;

            if (workersList.Count == 0)
            {
                MessageBox.Show("Записи за заданими критеріями не знайдено.");
            }
            else
            {
                MessageBox.Show("Відбір даних виконано успішно.");
            }
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDataFromDb();

            SearchWorkshopTextBox.Text = "";
            SearchSurnameTextBox.Text = "";

            MessageBox.Show("Повний список записів знову відображено.");
        }
    }
}