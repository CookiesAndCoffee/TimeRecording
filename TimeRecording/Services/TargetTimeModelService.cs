using Microsoft.EntityFrameworkCore;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;
using TimeRecording.Setup;

namespace TimeRecording.Services
{
    public class TargetTimeModelService : BaseService<TargetTimeModel>, ITargetTimeModelService
    {
        public TargetTimeModelService(TimeRecordingDBContext dbContext) : base(dbContext)
        {
        }

        public void SaveTargetTimeModelTimes(TargetTimeModelTimes modelTimes)
        {
            try
            {
                _dbContext.TargetTimeModelTimes.Add(modelTimes);
            }
            catch (DbUpdateException)
            {
                _dbContext.TargetTimeModelTimes.Update(modelTimes);
            }
            _dbContext.SaveChanges();
        }

        public List<TargetTimeModelTimes> GetTargetTimeModelTimes()
        {
            return _dbContext.TargetTimeModelTimes.ToList();
        }

        protected override DbSet<TargetTimeModel> GetSet()
        {
            return _dbContext.TargetTimeModel;
        }

        /// <summary>
        /// Delete the entity and every reference
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(TargetTimeModel entity)
        {
            DeleteReferences<TargetTimeModelTimes>(entity);
            DeleteReferences<PersonTargetTimeModel>(entity);
            base.Delete(entity);
        }
    }
}
