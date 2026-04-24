using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lab9_RPM
{
    public class Contact : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _phone = string.Empty;

        public Contact(string name, string phone)
        {
            _name = name;
            _phone = phone;

            // Валидация при создании контакта
            if (!Validate())
            {
                throw new ArgumentException("Некорректные данные контакта");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Validate()
        {
            // Проверка: имя не должно быть пустым
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            // Проверка: номер телефона не должен быть пустым
            if (string.IsNullOrWhiteSpace(Phone))
                return false;

            string phonePattern = @"^(\+7|8)?\d{10}$";
            return Regex.IsMatch(Phone, phonePattern);
        }

        public override string ToString()
        {
            return $"{Name} - {Phone}";
        }
    }
}
