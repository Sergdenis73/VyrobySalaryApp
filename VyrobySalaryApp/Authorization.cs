using System;
using System.Collections.Generic;
using System.Text;

namespace VyrobySalaryApp
{
    public class Authorization
    {
        public static string CurrentRole = "Керівник";

        public static bool LogCheck(string login, string password)
        {
            if (login == "buhgalter" && password == "111")
            {
                CurrentRole = "Бухгалтер";
                return true;
            }

            if (login == "kerivnyk" && password == "222")
            {
                CurrentRole = "Керівник";
                return true;
            }

            CurrentRole = "Керівник";
            return false;
        }
    }
}