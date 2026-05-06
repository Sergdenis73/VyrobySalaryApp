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

            workersList = new ObservableCollection<WorkerProduct>();

            workersList.Add(new WorkerProduct
            {
                id = 1,
                surname = "Денисенко",
                workshop_number = 1,
                product_a = 12,
                product_b = 8,
                product_c = 5
            });

            workersList.Add(new WorkerProduct
            {
                id = 2,
                surname = "Шкурат",
                workshop_number = 1,
                product_a = 10,
                product_b = 6,
                product_c = 7
            });

            workersList.Add(new WorkerProduct
            {
                id = 3,
                surname = "Іваненко",
                workshop_number = 2,
                product_a = 15,
                product_b = 9,
                product_c = 4
            });

            workersList.Add(new WorkerProduct
            {
                id = 4,
                surname = "Петренко",
                workshop_number = 2,
                product_a = 7,
                product_b = 11,
                product_c = 6
            });

            ProductsDataGrid.ItemsSource = workersList;
        }
    }
}