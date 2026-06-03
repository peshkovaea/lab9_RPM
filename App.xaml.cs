using lab9_RPM.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace lab9_RPM
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Регистрация DbContext
            services.AddDbContext<PeshkovaEA_RPM_lab12Context>(options =>
                options.UseSqlServer("Data Source=DBSRV\\ag2025;Initial Catalog=PeshkovaEA_RPM_lab12;Integrated Security=True;TrustServerCertificate=True"),
                ServiceLifetime.Scoped);

            // Регистрация сервисов
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Регистрация ViewModels
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<MainWindowViewModel>();

            // Регистрация MainWindow
            services.AddSingleton<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.Show();
        }
    }
}