using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9_RPM.Services
{
    // <summary>
    /// Реализация сервиса навигации
    /// </summary>
    /// <summary>
    /// Реализация сервиса навигации
    /// </summary>
    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private object _currentViewModel;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            private set => Set(ref _currentViewModel, value);
        }

        /// <summary>
        /// Получить текущую ViewModel
        /// </summary>
        public object GetCurrentViewModel()
        {
            return _currentViewModel;
        }

        /// <summary>
        /// Выполняет переход к указанной ViewModel
        /// </summary>
        public void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class
        {
            // 1. Получение экземпляра ViewModel из DI-контейнера
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

            // 2. Если ViewModel поддерживает интерфейс INavigationAware, передаём параметр
            if (viewModel is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(parameter);
            }

            // 3. Обновляем текущую ViewModel (ContentControl автоматически обновит содержимое)
            CurrentViewModel = viewModel;
        }
    }
}
