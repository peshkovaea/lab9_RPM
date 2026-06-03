
using lab9_RPM.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Input;

namespace lab9_RPM.ViewModels
{
	public class ContactEditViewModel : ObservableObject, INavigationAware
	{
		private readonly PeshkovaEA_RPM_lab12Context _context;
		private readonly IDialogService _dialogService;
		private readonly INavigationService _navigationService;

		private Contacts _editingContact;
		private string _name;
		private string _phone;
		private bool _isEditMode;

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

		public bool IsEditMode
		{
			get => _isEditMode;
			set => Set(ref _isEditMode, value);
		}

		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }

		public ContactEditViewModel(
			PeshkovaEA_RPM_lab12Context context,
			IDialogService dialogService,
			INavigationService navigationService)
		{
			_context = context;
			_dialogService = dialogService;
			_navigationService = navigationService;

			SaveCommand = new RelayCommand(Save, CanSave);
			CancelCommand = new RelayCommand(Cancel);
		}

		public void OnNavigatedTo(object parameter)
		{
			if (parameter is Contacts contact)
			{
				// Режим редактирования
				IsEditMode = true;
				_editingContact = contact;
				Name = contact.Name;
				Phone = contact.Phone;
			}
			else
			{
				// Режим добавления
				IsEditMode = false;
				_editingContact = null;
				Name = string.Empty;
				Phone = string.Empty;
			}
		}

		private bool CanSave()
		{
			return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
		}

		private void Save()
		{
			try
			{
				if (IsEditMode)
				{
					//  Реализация операции редактирования (Update)
					UpdateContact();
				}
				else
				{
					// Реализация операции создания (Create)
					CreateContact();
				}

				_navigationService.NavigateTo<ContactsListViewModel>();
			}
			catch (DbUpdateException ex)
			{
				_dialogService.ShowError($"Ошибка базы данных: {ex.InnerException?.Message ?? ex.Message}");
			}
			catch (Exception ex)
			{
				_dialogService.ShowError($"Ошибка: {ex.Message}");
			}
		}

		private void CreateContact()
		{
			// Проверка на дубликат
			if (_context.Contacts.Any(c => c.Phone == Phone.Trim()))
			{
				_dialogService.ShowWarning("Контакт с таким номером телефона уже существует!");
				return;
			}

			var newContact = new Contacts
			{
				Name = Name.Trim(),
				Phone = Phone.Trim()
			};

			// 1. Помечаем объект как добавленный
			_context.Contacts.Add(newContact);

			// 2. Сохраняем изменения в БД (генерирует INSERT)
			_context.SaveChanges();

			_dialogService.ShowInfo("Контакт успешно добавлен!");
		}

		private void UpdateContact()
		{
			// Проверка на дубликат (исключая текущий контакт)
			if (_context.Contacts.Any(c => c.Phone == Phone.Trim() && c.Id != _editingContact.Id))
			{
				_dialogService.ShowWarning("Контакт с таким номером телефона уже существует!");
				return;
			}

			// Объект _editingContact уже отслеживается контекстом
			_editingContact.Name = Name.Trim();
			_editingContact.Phone = Phone.Trim();

			// SaveChanges обнаружит изменения и сгенерирует UPDATE
			_context.SaveChanges();

			_dialogService.ShowInfo("Контакт успешно обновлён!");
		}

		private void Cancel()
		{
			if (_dialogService.ShowConfirmation("Отменить изменения?", "Подтверждение"))
			{
				_navigationService.NavigateTo<ContactsListViewModel>();
			}
		}
	}
}