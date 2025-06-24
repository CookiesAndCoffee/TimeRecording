using Microsoft.Extensions.DependencyInjection;
using TimeRecording.Services;
using TimeRecording.ViewModels;
using TimeRecording.Views;

namespace TimeRecording.Setup
{
    /// <summary>
    /// Provides methods to register DbContexts and transient services in the DI container.
    /// </summary>
    public static class ServiceRegistrar
    {
        public static void RegisterDbContexts(IServiceCollection services)
        {
            services.AddDbContext<TimeRecordingDBContext>();
        }

        public static void RegisterTransients(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            //TimeRecording Content
            services.AddTransient<TimeRecordingView>();
            services.AddTransient<TimeRecordingViewModel>();
            services.AddTransient<TimeRecordingService>();
        }
    }
}