using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DoroshenkoLab2
{
    internal class User
    {
        private DateTime _birthDate;
        private string _name;
        private string _surname;
        private string _email;

        internal DateTime BirthDate
        {
            get { return _birthDate; }
            private set
            {
                int date = (DateTime.Today - value).Days;
                if (date < 0)
                    throw new FutureDateException(value);
                int day = date / 365;
                if (day > 110)
                    throw new PastDateException(value);
                _birthDate = value;
            }
        }
        internal string Name
        {
            get { return _name; }
            private set
            {
                if (Regex.IsMatch(value, @"^[a-zA-Z'-]+$"))
                    _name = value;
                else
                    throw new InvalidNameException($"{value} {_surname}");
            }
        }

        internal string Surname
        {
            get { return _surname; }
            private set
            {
                if (Regex.IsMatch(value, @"^[a-zA-Z'-]+$"))
                    _surname = value;
                else
                    throw new InvalidNameException($"{_name} {value}");
            }
        }

        internal string Email
        {
            get { return _email; }
            private set
            {
                if (new EmailAddressAttribute().IsValid(value))
                    _email = value;
                else
                    throw new InvalidEmailException(value);
            }
        }

        internal bool IsAdult { get; }
        internal bool IsBirthday { get; }
        internal int Age { get; }
        internal string ChineseSign { get; }
        internal string SunSign { get; }

        private int AgeAdultCalc()
        {
            DateTime today = DateTime.Today;
            return today.Year - BirthDate.Year - (today.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
        }

        private string CalcChSign()
        {
            switch (BirthDate.Year % 12)
            {
                case 0:
                    return "Monkey";
                case 1:
                    return "Rooster";
                case 2:
                    return "Dog";
                case 3:
                    return "Pig";
                case 4:
                    return "Rat";
                case 5:
                    return "Ox";
                case 6:
                    return "Tiger";
                case 7:
                    return "Rabbit";
                case 8:
                    return "Dragon";
                case 9:
                    return "Snake";
                case 10:
                    return "Horse";
                case 11:
                    return "Sheep";
                default:
                    throw new ArgumentException();
            }
        }

        private string CalcSunSign()
        {
            switch (BirthDate.Month)
            {
                case 1:
                    return BirthDate.Day <= 19 ? "Capricorn" : "Aquarius";
                case 2:
                    return BirthDate.Day <= 17 ? "Aquarius" : "Pisces";
                case 3:
                    return BirthDate.Day <= 19 ? "Pisces" : "Aries";
                case 4:
                    return BirthDate.Day <= 19 ? "Aries" : "Taurus";
                case 5:
                    return BirthDate.Day <= 20 ? "Taurus" : "Gemini";
                case 6:
                    return BirthDate.Day <= 20 ? "Gemini" : "Cancer";
                case 7:
                    return BirthDate.Day <= 22 ? "Cancer" : "Leo";
                case 8:
                    return BirthDate.Day <= 22 ? "Leo" : "Virgo";
                case 9:
                    return BirthDate.Day <= 22 ? "Virgo" : "Libra";
                case 10:
                    return BirthDate.Day <= 22 ? "Libra" : "Scorpio";
                case 11:
                    return BirthDate.Day <= 21 ? "Scorpio" : "Sagittarius";
                case 12:
                    return BirthDate.Day <= 21 ? "Sagittarius" : "Capricorn";
                default:
                    throw new ArgumentException();
            }
        }

        private User(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }

        private User(string name, string surname, DateTime birthDate)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;            
            IsAdult = (Age = AgeAdultCalc()) > 18;
            IsBirthday = BirthDate.DayOfYear == DateTime.Today.DayOfYear;
            ChineseSign = CalcChSign();
            SunSign = CalcSunSign();
        }

        internal User(string name, string surname, string email, DateTime birthDate) : this(name, surname, birthDate)
        {
            Email = email;
        }


    }



    internal class InvalidEmailException : Exception
    {
        internal InvalidEmailException(string email) : base($"Invalid Email: {email}") { }
    }

    internal class InvalidNameException : Exception
    {
        internal InvalidNameException(string name) : base($"Name is incorrect: {name}") { }
    }

    internal class FutureDateException : Exception
    {
        internal FutureDateException(DateTime date) : base($"Are you from future !?: {date.ToShortDateString()}") { }
    }

    internal class PastDateException : Exception
    {
        internal PastDateException(DateTime date) : base($"You are too old: {date.ToShortDateString()}") { }
    }


}
