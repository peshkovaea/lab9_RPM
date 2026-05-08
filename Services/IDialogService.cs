using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9_RPM.Services
{
    // Интерфейс сервиса диалоговых окон
    public interface IDialogService
    {
        /// <summary>
        /// Показать информационное сообщение
        /// </summary>
        void ShowInfo(string message, string title = "Информация");

        /// <summary>
        /// Показать предупреждение
        /// </summary>
        void ShowWarning(string message, string title = "Предупреждение");

        /// <summary>
        /// Показать сообщение об ошибке
        /// </summary>
        void ShowError(string message, string title = "Ошибка");

        /// <summary>
        /// Показать диалог подтверждения (Да/Нет)
        /// </summary>
        /// <returns>True - если пользователь нажал Да, False - если Нет</returns>
        bool ShowConfirmation(string message, string title = "Подтверждение");
    }
}