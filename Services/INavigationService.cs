using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9_RPM.Services
{
    /// <summary>
    /// Сервис навигации между экранами приложения
    /// Реализует подход ViewModel-First
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Текущая ViewModel (содержимое ContentControl)
        /// </summary>
        object CurrentViewModel { get; }

        /// <summary>
        /// Получить текущую ViewModel (альтернативный метод)
        /// </summary>
        object GetCurrentViewModel();

        /// <summary>
        /// Переход к указанной ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel для перехода</typeparam>
        /// <param name="parameter">Параметр, передаваемый в ViewModel</param>
        void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class;
    }
}
