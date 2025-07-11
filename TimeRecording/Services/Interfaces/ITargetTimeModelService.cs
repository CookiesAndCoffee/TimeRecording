using TimeRecording.Models;

namespace TimeRecording.Services.Interfaces
{
    public interface ITargetTimeModelService : IService<TargetTimeModel>
    {   
        void SaveTargetTimeModelTimes(TargetTimeModelTimes modelTimes);

        List<TargetTimeModelTimes> GetTargetTimeModelTimes();
    }
}
