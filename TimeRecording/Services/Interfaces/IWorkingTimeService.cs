using TimeRecording.Models;

namespace TimeRecording.Services.Interfaces
{
    public interface IWorkingTimeService : IService<WorkingTime>
    {
        /// <summary>
        /// Returnes the working time for a specific person and date.
        /// </summary>
        WorkingTime? GetWorkingTimeForDate(Person person, DateTime date);
    }
}
