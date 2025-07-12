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

        public void SaveTargetTimeModelTimes(TargetTimeModel model, List<TargetTimeModelTimes> modelTimes)
        {
            SavesReferences<TargetTimeModelTimes>(model, modelTimes);
        }

        public List<TargetTimeModelTimes> GetTargetTimeModelTimes(TargetTimeModel model)
        {
            return _dbContext.TargetTimeModelTimes.Where<TargetTimeModelTimes>(ttmt => ttmt.TargetTimeModelId == model.Id).ToList();
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
