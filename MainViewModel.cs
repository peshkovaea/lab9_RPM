using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using lab9_RPM.Services;

namespace lab9_RPM
{
    /// <summary>
    /// Главная ViewModel приложения "Телефонная книга"
    /// Внедрение зависимостей через конструктор (Constructor Injection)
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;

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

        /// <summary>
        /// Constructor Injection - DI-контейнер автоматически передаст реализацию IDialogService
        /// </summary>
        /// <param name="dialogService">Сервис диалоговых окон</param>
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));//проверка на нуль гарантирует что _dialogServicе никогдда не будет null

            Contacts = new ObservableCollection<Contact>();

            // Инициализация команд
            AddCommand = new RelayCommand(AddContact, () => CanAddContact());
            DeleteCommand = new RelayCommand<Contact>(DeleteContact, (contact) => CanDeleteContact(contact));

            // Добавление тестовых данных
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            Contacts.Add(new Contact("Иван Петров", "+79161234567"));
            Contacts.Add(new Contact("Мария Сидорова", "89261234567"));
            Contacts.Add(new Contact("Алексей Иванов", "9123456789"));
        }

        /// <summary>
        /// Добавление нового контакта
        /// </summary>
        private void AddContact()
        {
            try
            {
                // Проверка на дубликат по номеру телефона
                if (Contacts.Any(c => c.Phone == Phone.Trim()))
                {
                    _dialogService.ShowWarning("Контакт с таким номером телефона уже существует!");
                    return;
                }

                // Создание нового контакта
                var newContact = new Contact(Name.Trim(), Phone.Trim());
                Contacts.Add(newContact);

                // Информационное сообщение об успешном добавлении
                _dialogService.ShowInfo($"Контакт \"{Name.Trim()}\" успешно добавлен!");

                // Очистка полей ввода
                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (ArgumentException ex)
            {
                // Обработка ошибок валидации
                _dialogService.ShowWarning(ex.Message, "Ошибка добавления контакта");
            }
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }

        /// <summary>
        /// Удаление выбранного контакта с подтверждением
        /// </summary>
        private void DeleteContact(Contact contact)
        {
            if (contact == null || !Contacts.Contains(contact))
                return;

            // Запрос подтверждения удаления
            if (_dialogService.ShowConfirmation($"Вы действительно хотите удалить контакт \"{contact.Name}\"?", "Подтверждение удаления"))
            {
                Contacts.Remove(contact);

                // Очистка выбранного контакта после удаления
                if (SelectedContact == contact)
                {
                    SelectedContact = null;
                }

                _dialogService.ShowInfo($"Контакт \"{contact.Name}\" удалён.");
            }
        }

        private bool CanDeleteContact(Contact contact)
        {
            return contact != null && Contacts.Contains(contact);
        }
    }
}