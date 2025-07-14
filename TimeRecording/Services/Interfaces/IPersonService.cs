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

        /// <summary>
        /// Returns the target time model times for a specific person and date.
        /// </summary>
        TargetTimeModelTimes? GetTargetTimeModelTimesForDate(Person person, DateTime date);

        /// <summary>
        /// Returns the target time for a specific person and date range.
        /// </summary>
        int GetTargetTimeForMonth(Person person, DateTime month);

        /// <summary>
        /// Calculates the monthly balance for a person based on their target time and actual time recorded.
        /// </summary>
        int GetMonthlyBalance(Person person, DateTime monat);
    }
}
