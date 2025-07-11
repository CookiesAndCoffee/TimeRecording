using Microsoft.EntityFrameworkCore;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;
using TimeRecording.Setup;

namespace TimeRecording.Services
{
    public class PersonService : BaseService<Person>, IPersonService
    {
        public PersonService(TimeRecordingDBContext dbContext) : base(dbContext)
        {
        }

        protected override DbSet<Person> GetSet()
        {
            return _dbContext.Person;
        }

        /// <summary>
        /// Delete the entity and every reference
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(Person entity)
        {
            DeleteReferences<WorkingTime>(entity);
            DeleteReferences<PersonTargetTimeModel>(entity);
            base.Delete(entity);
        }
    }
}
