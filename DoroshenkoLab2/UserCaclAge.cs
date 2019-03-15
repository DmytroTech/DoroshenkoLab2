using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroshenkoLab2
{
    internal static class UserCalcAge
    {
        internal static User CreateUser(string name, string surname, string email, DateTime date)
        {
            return new User(name, surname, email, date);
        }
    }
}
