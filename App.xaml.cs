using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using lab9_RPM.Services;
using lab9_RPM.ViewModels;

namespace lab9_RPM
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Сервисы
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDialogService, DialogService>();

            services.AddTransient<ContactsListViewModel>();  
            services.AddTransient<AboutViewModel>();
            services.AddSingleton<MainWindowViewModel>();

            // Главное окно
            services.AddSingleton<MainWindow>(provider =>
            {
                var window = new MainWindow();
                window.DataContext = provider.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.GetRequiredService<MainWindow>().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}