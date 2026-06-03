
using lab9_RPM.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace lab9_RPM.ViewModels
{
    public class ContactsListViewModel : ObservableObject, INavigationAware
    {
        private readonly PeshkovaEA_RPM_lab12Context _context;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Contacts> Contacts { get; set; }

        private Contacts _selectedContact;
        private string _searchText;

        public Contacts SelectedContact
        {
            get => _selectedContact;
            set
            {
                Set(ref _selectedContact, value);
                // Обновляем команды при изменении выделения
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (Set(ref _searchText, value))
                {
                    LoadContacts(); // Фильтрация при изменении текста поиска
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ContactsListViewModel(
            PeshkovaEA_RPM_lab12Context context,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _context = context;
            _dialogService = dialogService;
            _navigationService = navigationService;

            Contacts = new ObservableCollection<Contacts>();

            AddCommand = new RelayCommand(AddContact);
            EditCommand = new RelayCommand(EditContact, () => SelectedContact != null);
            DeleteCommand = new RelayCommand(DeleteContact, () => SelectedContact != null);

            LoadContacts();
        }

        public void OnNavigatedTo(object parameter)
        {
            // Обновляем список при возврате к этому ViewModel
            LoadContacts();
        }

        private void LoadContacts()
        {
            try
            {
                var query = _context.Contacts.AsQueryable();

                // Реализация фильтрации (поиска)
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    query = query.Where(c => c.Name.Contains(SearchText) ||
                                             c.Phone.Contains(SearchText));
                }

                var contactsList = query.OrderBy(c => c.Name).ToList();

                Contacts.Clear();
                foreach (var contact in contactsList)
                {
                    Contacts.Add(contact);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка загрузки контактов: {ex.Message}");
            }
        }

        private void AddContact()
        {
            _navigationService.NavigateTo<ContactEditViewModel>(null);
        }

        private void EditContact()
        {
            if (SelectedContact != null)
            {
                _navigationService.NavigateTo<ContactEditViewModel>(SelectedContact);
            }
        }

        private void DeleteContact()
        {
            if (SelectedContact == null) return;

            if (_dialogService.ShowConfirmation($"Удалить контакт \"{SelectedContact.Name}\"?", "Подтверждение"))
            {
                try
                {
                    // Реализация операции удаления (Delete)
                    _context.Contacts.Remove(SelectedContact);
                    _context.SaveChanges();
                    Contacts.Remove(SelectedContact);

                    _dialogService.ShowInfo("Контакт успешно удалён");
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError($"Ошибка при удалении: {ex.Message}");
                }
            }
        }
    }
}