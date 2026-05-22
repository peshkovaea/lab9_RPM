using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9_RPM.ViewModels
{
    // <summary>
    /// ViewModel для экрана "О программе"
    /// </summary>
    public class AboutViewModel
    {
        /// <summary>
        /// Название приложения
        /// </summary>
        public string AppName => "Телефонная книга MVVM";

        /// <summary>
        /// Версия приложения
        /// </summary>
        public string Version => "Версия 2.0";

        /// <summary>
        /// Информация о разработчике
        /// </summary>
        public string Developer => "Разработана лабораторная работа №11";

        /// <summary>
        /// Год создания
        /// </summary>
        public string Year => "2026";
    }
}