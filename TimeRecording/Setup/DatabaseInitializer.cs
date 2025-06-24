using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TimeRecording.Setup
{
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Applies all pending migrations and creates the database if it does not exist.        
        /// </summary>
        /// <param name="serviceProvider">
        /// The application's ServiceProvider.
        /// </param>
        public static void EnsureDatabaseMigrated(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TimeRecordingDBContext>();
            dbContext.Database.Migrate();
        }
    }
}