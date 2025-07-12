using TimeRecording.Models;

namespace TimeRecording.Services.Interfaces
{
    public interface IPersonService : IService<Person>
    {
        /// <summary>
        /// Saves all the References of the given person.
        /// </summary>
        void SavePersonTargetTimeModel(Person person, List<PersonTargetTimeModel> models);

        /// <summary>
        /// Get all the References of the given person.
        /// </summary>
        List<PersonTargetTimeModel> GetPersonTargetTimeModels(Person person);
    }
}
