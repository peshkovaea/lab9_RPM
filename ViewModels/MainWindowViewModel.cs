using lab9_RPM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace lab9_RPM.ViewModels
{
    // <summary>
    /// ViewModel для главного окна (Shell)
    /// Управляет навигацией через меню
    /// </summary>
    public class MainWindowViewModel
    {
        private readonly INavigationService _navigationService;

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Инициализация команд
            ShowContactsCommand = new RelayCommand(() =>
                _navigationService.NavigateTo<ContactsListViewModel>());

            ShowAboutCommand = new RelayCommand(() =>
                _navigationService.NavigateTo<AboutViewModel>());

            // При запуске открываем список контактов
            _navigationService.NavigateTo<ContactsListViewModel>();
        }

        /// <summary>
        /// Команда открытия списка контактов
        /// </summary>
        public ICommand ShowContactsCommand { get; }

        /// <summary>
        /// Команда открытия окна "О программе"
        /// </summary>
        public ICommand ShowAboutCommand { get; }

        /// <summary>
        /// Сервис навигации (доступен для привязки в XAML)
        /// </summary>
        public INavigationService NavigationService => _navigationService;
    }
}
