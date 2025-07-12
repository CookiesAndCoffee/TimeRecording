using TimeRecording.Models;

namespace TimeRecording.Services.Interfaces
{
    public interface ITargetTimeModelService : IService<TargetTimeModel>
    {
        /// <summary>
        /// Saves the target time model times for a specific target time model.
        /// </summary>
        void SaveTargetTimeModelTimes(TargetTimeModel model, List<TargetTimeModelTimes> modelTimes);

        /// <summary>
        /// Gets all target time model times for a specific target time model.
        /// </summary>
        List<TargetTimeModelTimes> GetTargetTimeModelTimes(TargetTimeModel model);
    }
}
