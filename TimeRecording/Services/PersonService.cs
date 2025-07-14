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

        public void SavePersonTargetTimeModel(Person person, List<PersonTargetTimeModel> models)
        {
            var existingModels = GetPersonTargetTimeModels(person);
            foreach (var model in existingModels)
                if (!models.Any(m => m.Equals(model)))
                    _dbContext.PersonTargetTimeModel.Remove(model);
            SavesReferences<PersonTargetTimeModel>(person, models);
        }

        public List<PersonTargetTimeModel> GetPersonTargetTimeModels(Person person)
        {
            return _dbContext.PersonTargetTimeModel.Where<PersonTargetTimeModel>(pttm => pttm.PersonId == person.Id).ToList();
        }

        private List<PersonTargetTimeModel> GetPersonTargetTimeModels(Person person, DateTime until)
        {
            return _dbContext.PersonTargetTimeModel
                .Include(psm => psm.TargetTimeModel)
                .Where(pttm => pttm.PersonId == person.Id && pttm.ValidFrom <= until)
                .OrderByDescending(pttm => pttm.ValidFrom)
                .ToList();
        }

        private List<TargetTimeModelTimes> GetTargetTimeModelTimes(Person person, DateTime until)
        {
            var personsTargetTimes = GetPersonTargetTimeModels(person, until).Select(pttm => pttm.TargetTimeModelId).ToList();
            if (personsTargetTimes.Count > 0)
            {
                return _dbContext.TargetTimeModelTimes
                    .Where(ttmt => personsTargetTimes.Contains(ttmt.TargetTimeModelId) && ttmt.ValidFrom <= until)
                    .OrderByDescending(ttmt => ttmt.ValidFrom)
                    .ToList();
            }
            return new List<TargetTimeModelTimes>();
        }

        private List<TargetTimeModelTimes> GetTargetTimeModelTimes(Person person, DateTime since, DateTime until)
        {
            var results = GetTargetTimeModelTimes(person, until);
            var first = results.FirstOrDefault(ttmt => ttmt.ValidFrom <= since);
            if (first != null)
                results.RemoveAll(ttmt => ttmt.ValidFrom < first.ValidFrom);
            return results;
        }

        public TargetTimeModelTimes? GetTargetTimeModelTimesForDate(Person person, DateTime date)
        {
            return GetTargetTimeModelTimes(person, date, date).FirstOrDefault();
        }

        public int GetTargetTimeForMonth(Person person, DateTime month)
        {
            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var personTargetTimeModels = GetPersonTargetTimeModels(person, lastDayOfMonth);
            var targetTimeModelTimes = GetTargetTimeModelTimes(person, firstDayOfMonth, lastDayOfMonth);
            int sum = 0;
            if (targetTimeModelTimes.Count > 0 && personTargetTimeModels.Count > 0)
            {
                int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
                for (int day = daysInMonth; day > 0; day--)
                {
                    var date = new DateTime(month.Year, month.Month, day);
                    var personTargetTime = personTargetTimeModels.Where(pttm => pttm.ValidFrom <= date)
                                                    .OrderByDescending(pttm => pttm.ValidFrom)
                                                    .FirstOrDefault();
                    var timeModelTime = targetTimeModelTimes.FirstOrDefault(ttmt => ttmt.TargetTimeModelId <= personTargetTime?.TargetTimeModelId); ;
                    if (timeModelTime == null)
                        throw new Exception("Expected valid Time Model");
                    sum += timeModelTime.ForDate(date);
                }
            }
            return sum;
        }

        public int GetMonthlyBalance(Person person, DateTime monat)
        {
            int daysInMonth = DateTime.DaysInMonth(monat.Year, monat.Month);
            int balance = 0;
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(monat.Year, monat.Month, day);
                var workingTime = _dbContext.WorkingTime
                    .FirstOrDefault(a => a.PersonId == person.Id && a.Date == date);
                int minutes = workingTime?.Minutes ?? 0;
                balance += minutes;
            }
            return balance - GetTargetTimeForMonth(person, monat);
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
