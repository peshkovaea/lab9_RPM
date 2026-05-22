using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using lab9_RPM.Services;

namespace lab9_RPM.ViewModels
{
    public class ContactsListViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Contact> Contacts { get; }

        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact _selectedContact;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }

        public Contact SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public ContactsListViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;

            Contacts = new ObservableCollection<Contact>();
            AddCommand = new RelayCommand(AddContact, () => CanAddContact());
            DeleteCommand = new RelayCommand<Contact>(DeleteContact, contact => CanDeleteContact(contact));

            // Добавляем тестовые данные
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            Contacts.Add(new Contact("Иван Петров", "+79161234567"));
            Contacts.Add(new Contact("Мария Сидорова", "89261234567"));
            Contacts.Add(new Contact("Алексей Иванов", "9123456789"));
        }

        private void AddContact()
        {
            try
            {
                // Проверка на дубликат
                if (Contacts.Any(c => c.Phone == Phone.Trim()))
                {
                    _dialogService.ShowWarning("Контакт с таким номером телефона уже существует!");
                    return;
                }

                var newContact = new Contact(Name.Trim(), Phone.Trim());
                Contacts.Add(newContact);
                _dialogService.ShowInfo($"Контакт \"{Name.Trim()}\" успешно добавлен!");

                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (ArgumentException ex)
            {
                _dialogService.ShowWarning(ex.Message, "Ошибка добавления контакта");
            }
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }

        private void DeleteContact(Contact contact)
        {
            if (contact == null || !Contacts.Contains(contact))
                return;

            if (_dialogService.ShowConfirmation($"Удалить контакт \"{contact.Name}\"?", "Подтверждение"))
            {
                Contacts.Remove(contact);
                if (SelectedContact == contact) SelectedContact = null;
            }
        }

        private bool CanDeleteContact(Contact contact)
        {
            return contact != null && Contacts.Contains(contact);
        }
    }
}