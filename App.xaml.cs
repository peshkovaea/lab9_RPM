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

            services.AddDbContext<PeshkovaEA_RPM_lab12Context>(options =>
                options.UseSqlServer("Data Source=DBSRV\\ag2025;Initial Catalog=PeshkovaEA_RPM_lab12;Integrated Security=True;TrustServerCertificate=True"));

            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<MainViewModel>();  

            services.AddSingleton<MainWindow>(provider =>
            {
                var window = new MainWindow();
                window.DataContext = provider.GetRequiredService<MainViewModel>();
                return window;
            });

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _serviceProvider?.Dispose();
        }
    }
}