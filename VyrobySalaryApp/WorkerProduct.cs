using System;
using System.Collections.Generic;
using System.Text;

namespace VyrobySalaryApp
{
    public class WorkerProduct
    {
        public int id { get; set; }

        public string surname { get; set; } = "";

        public int workshop_number { get; set; }

        public int product_a { get; set; }

        public int product_b { get; set; }

        public int product_c { get; set; }

        public double Salary
        {
            get
            {
                return (product_a * 500) + (product_b * 750) + (product_c * 1000);
            }
        }
    }
}