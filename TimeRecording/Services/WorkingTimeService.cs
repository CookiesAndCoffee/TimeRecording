using Microsoft.EntityFrameworkCore;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;
using TimeRecording.Setup;

namespace TimeRecording.Services
{
    public class WorkingTimeService : BaseService<WorkingTime>, IWorkingTimeService
    {
        public WorkingTimeService(TimeRecordingDBContext context) : base(context)
        {
        }

        protected override DbSet<WorkingTime> GetSet()
        {
            return _dbContext.WorkingTime;
        }

        public WorkingTime? GetWorkingTimeForDate(Person person, DateTime date)
        {
            return GetSet()
                .FirstOrDefault(wt => wt.Date == date && person.Id == wt.PersonId);
        }
    }
}
