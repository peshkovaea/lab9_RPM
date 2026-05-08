using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using lab9_RPM.Services;

namespace lab9_RPM
{
    /// <summary>
    /// Точка входа приложения с настройкой Dependency Injection
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Создание коллекции сервисов
            var services = new ServiceCollection();

            // 2. Регистрация сервисов с указанием времени жизни (Lifetime)

            // DialogService - Singleton (один экземпляр на всё приложение)
           
            services.AddSingleton<IDialogService, DialogService>();

            // MainViewModel - Transient (новый экземпляр при каждом запросе)
          
            services.AddTransient<MainViewModel>();

            // 3. Регистрация главного окна с явным указанием DataContext
            services.AddSingleton<MainWindow>(provider =>
            {
                var window = new MainWindow();
                // Устанавливаем DataContext из контейнера
                window.DataContext = provider.GetRequiredService<MainViewModel>();
                return window;
            });

            // 4. Построение контейнера (ServiceProvider)
            _serviceProvider = services.BuildServiceProvider();

            // 5. Получение главного окна и запуск приложения
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            // Освобождение ресурсов контейнера
            _serviceProvider?.Dispose();
        }
    }
}