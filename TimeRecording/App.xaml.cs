using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TimeRecording.Setup;

namespace TimeRecording
{
    /// <summary>
    /// Start up class for the application with dependency injection setup.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Initialize the service collection and register services
            var services = new ServiceCollection();
            ServiceRegistrar.RegisterDbContexts(services);
            ServiceRegistrar.RegisterTransients(services);
            ServiceProvider = services.BuildServiceProvider();
            // Ensures the database is migrated to the latest version
            DatabaseInitializer.EnsureDatabaseMigrated(ServiceProvider);
            base.OnStartup(e);

            ServiceProvider.GetService<MainWindow>()?.Show();
        }
    }

}
