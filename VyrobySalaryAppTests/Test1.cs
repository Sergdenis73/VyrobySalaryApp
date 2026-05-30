using Microsoft.VisualStudio.TestTools.UnitTesting;
using VyrobySalaryApp;

namespace VyrobySalaryAppTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void SelectByWorkshop_WhenWorkshopNumberIs1_ReturnsOnlyWorkshop1()
        {
            DataAccess dataAccess = new DataAccess();
            int workshopNumber = 1;

            var actual = dataAccess.SelectByWorkshop(workshopNumber);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0, "У БД немає записів для цеху №1.");

            foreach (WorkerProduct worker in actual)
            {
                Assert.AreEqual(workshopNumber, worker.workshop_number);
            }
        }

        [TestMethod]
        public void SelectByWorkshopAndSurname_WhenWorkshop1AndDenysenko_ReturnsOnlySelectedWorker()
        {
            DataAccess dataAccess = new DataAccess();
            int workshopNumber = 1;
            string surname = "Денисенко";

            var actual = dataAccess.SelectByWorkshopAndSurname(workshopNumber, surname);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0, "У БД немає записів із таким цехом і прізвищем.");

            foreach (WorkerProduct worker in actual)
            {
                Assert.AreEqual(workshopNumber, worker.workshop_number);
                Assert.AreEqual(surname, worker.surname);
            }
        }

        [TestMethod]
        public void Salary_WhenProductsAreSet_ReturnsCorrectSalary()
        {
            WorkerProduct worker = new WorkerProduct
            {
                product_a = 2,
                product_b = 3,
                product_c = 4
            };

            double expected = 2 * 500 + 3 * 750 + 4 * 1000;

            double actual = worker.Salary;

            Assert.AreEqual(expected, actual, 0.001);
        }
    }
}