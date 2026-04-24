using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace lab9_RPM
{
    public class MainViewModel : ObservableObject
    {
       
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


    public MainViewModel()
    {
        Contacts = new ObservableCollection<Contact>();

        // Инициализация команды добавления (без параметра)
        AddCommand = new RelayCommand(
            AddContact,
            () => CanAddContact()
        );

        // Инициализация команды удаления (с параметром - выбранный контакт)
        DeleteCommand = new RelayCommand<Contact>(
            DeleteContact,
            (contact) => CanDeleteContact(contact)
        );

        // Добавление тестовых данных для демонстрации
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
            // Создание нового контакта с введёнными данными
            var newContact = new Contact(Name.Trim(), Phone.Trim());
            Contacts.Add(newContact);

            // Очистка полей ввода после успешного добавления
            Name = string.Empty;
            Phone = string.Empty;
        }
        catch (ArgumentException ex)
        {
            // Обработка ошибок валидации
            System.Windows.MessageBox.Show(
                ex.Message,
                "Ошибка добавления контакта",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning
            );
        }
    }


    private bool CanAddContact()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(Phone);
    }

    private void DeleteContact(Contact contact)
    {
        if (contact != null && Contacts.Contains(contact))
        {
            Contacts.Remove(contact);

            // Очистка выбранного контакта после удаления
            if (SelectedContact == contact)
            {
                SelectedContact = null;
            }
        }
    }


    private bool CanDeleteContact(Contact contact)
    {
        return contact != null && Contacts.Contains(contact);
    }
}
}
