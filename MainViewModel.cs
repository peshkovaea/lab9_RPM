using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using lab9_RPM.Services;
using Microsoft.EntityFrameworkCore;

namespace lab9_RPM
{
    public class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly PeshkovaEA_RPM_lab12Context _context;

        public ObservableCollection<Contacts> Contacts { get; set; }

        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contacts _selectedContact;

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

        public Contacts SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel(IDialogService dialogService, PeshkovaEA_RPM_lab12Context context)
        {
            _dialogService = dialogService;
            _context = context;

            LoadContacts();

            AddCommand = new RelayCommand(AddContact, () => CanAddContact());
            DeleteCommand = new RelayCommand<Contacts>(DeleteContact, (contact) => CanDeleteContact(contact));
        }

        private void LoadContacts()
        {
            Contacts = new ObservableCollection<Contacts>(_context.Contacts.ToList());
        }

        private void AddContact()
        {
            try
            {
                if (Contacts.Any(c => c.Phone == Phone.Trim()))
                {
                    _dialogService.ShowWarning("Контакт с таким номером телефона уже существует!");
                    return;
                }

                var newContact = new Contacts { Name = Name.Trim(), Phone = Phone.Trim() };
                _context.Contacts.Add(newContact);
                _context.SaveChanges();
                Contacts.Add(newContact);

                _dialogService.ShowInfo($"Контакт \"{Name.Trim()}\" успешно добавлен!");

                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (Exception ex)
            {
                _dialogService.ShowWarning($"Ошибка: {ex.Message}");
            }
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }

        private void DeleteContact(Contacts contact)
        {
            if (contact == null) return;

            if (_dialogService.ShowConfirmation($"Удалить контакт \"{contact.Name}\"?", "Подтверждение"))
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
                Contacts.Remove(contact);

                if (SelectedContact == contact)
                    SelectedContact = null;

                _dialogService.ShowInfo($"Контакт \"{contact.Name}\" удалён.");
            }
        }

        private bool CanDeleteContact(Contacts contact)
        {
            return contact != null;
        }
    }
}