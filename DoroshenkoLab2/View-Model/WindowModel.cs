using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DoroshenkoLab2
{
    internal class WindowModel : INotifyPropertyChanged
    {
        private string _name = "";
        private string _surname = "";
        private string _email = "";
        private DateTime _date = DateTime.Today;


        private string _birthday;
        private string _adult;
        private string _wZodiac;
        private string _cZodiac;
        private bool _canExecute;

        private RelayCommand _calculatingCommand;
        private RelayCommand _backToRegist;


        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                CanExecute = CheckValues();
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                CanExecute = CheckValues();
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                CanExecute = CheckValues();
                OnPropertyChanged();
            }
        }

        public bool CanExecute
        {
            get { return _canExecute; }
            private set
            {
                _canExecute = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                CanExecute = CheckValues();
                OnPropertyChanged();
            }
        }

        public string BirthDate
        {
            get { return _date.ToShortDateString(); }
        }

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged();
            }
        }

        public string Adult
        {
            get { return _adult; }
            set
            {
                _adult = value;
                OnPropertyChanged();
            }
        }

        public string WestZ
        {
            get { return _wZodiac; }
            private set
            {
                _wZodiac = value;
                OnPropertyChanged();
            }
        }

        public string ChinZ
        {
            get { return _cZodiac; }
            private set
            {
                _cZodiac = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand CalculatingCommand
        {
            get { return _calculatingCommand ?? (_calculatingCommand = new RelayCommand(AgeCalculation)); }
        }

        public RelayCommand BackToRegistr
        {
            get { return _backToRegist ?? (_backToRegist = new RelayCommand(NullifyObjects)); }
        }


        private void NullifyValues()
        {
            Date = DateTime.Today;
            Name = "";
            Surname = "";
            Email = "";
        }

        private void NullifyOtherValues()
        {
            CanExecute = false;
            Birthday = "";
            Adult = "";
            WestZ = "";
            ChinZ = "";
        }

        private bool CheckValues()
        {
            return _name != "" && _surname != "" && _email != "";
        }

        private readonly Action _closeAction;

        private readonly Action _returnAction;

        private readonly Action<bool> _showLoaderAction;

        internal WindowModel(Action closeAction, Action returnAction, Action<bool> showLoader)
        {
            _returnAction = returnAction;
            _closeAction = closeAction;
            _showLoaderAction = showLoader;
            CanExecute = false;
        }


        private async void AgeCalculation(object o)
        {
            _showLoaderAction.Invoke(true);
            NullifyOtherValues();
            try
            {
                await Task.Run(() =>
                {
                    UserManager.CurrentUser = UserCalcAge.PersonCreated(_name, _surname, _email, _date);
                    Thread.Sleep(1000);
                });

                WestZ = UserManager.CurrentUser.SunSign;
                ChinZ = UserManager.CurrentUser.ChineseSign;
                Birthday = $"{UserManager.CurrentUser.IsBirthday}";
                Adult = $"{UserManager.CurrentUser.IsAdult}";
                
                if (DateTime.Today.DayOfYear == _date.DayOfYear)
                    MessageBox.Show($"Happy Birthday {Name}!");

                _closeAction.Invoke();
                CanExecute = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                NullifyValues();
            }
            _showLoaderAction.Invoke(false);
        }

        private async void NullifyObjects(object o)
        {
            await Task.Run(() =>
            {
                NullifyValues();
                NullifyOtherValues();
            });
            _returnAction.Invoke();
        }        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
