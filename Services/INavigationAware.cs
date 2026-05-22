using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9_RPM.Services
{
    /// <summary>
    /// Интерфейс для ViewModels, которые должны получать уведомления о навигации
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Вызывается при переходе к данной ViewModel
        /// </summary>
        /// <param name="parameter">Параметр, переданный при навигации</param>
        void OnNavigatedTo(object parameter);
    }
}
