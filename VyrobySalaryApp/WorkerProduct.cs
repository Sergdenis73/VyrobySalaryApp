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
    }
}