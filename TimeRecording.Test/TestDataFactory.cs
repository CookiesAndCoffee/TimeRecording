using TimeRecording.Models;

namespace TimeRecording.Test
{
    /// <summary>
    /// Provides flexible factory methods for test data for all models.
    /// </summary>
    public static class TestDataFactory
    {
        public static Person CreatePerson(Action<Person>? setup = null)
        {
            var person = new Person
            {
                Id = 1,
                PersonnelNumber = "1001",
                FirstName = "Max",
                LastName = "Mustermann"
            };
            setup?.Invoke(person);
            return person;
        }

        public static WorkingTime CreateWorkingTime(Action<WorkingTime>? setup = null)
        {
            var workingTime = new WorkingTime
            {
                PersonId = 1,
                Date = DateTime.Today,
                Minutes = 480
            };
            setup?.Invoke(workingTime);
            return workingTime;
        }

        public static TargetTimeModel CreateTargetTimeModel(Action<TargetTimeModel>? setup = null)
        {
            var model = new TargetTimeModel
            {
                Id = 1,
                Model = "Standard"
            };
            setup?.Invoke(model);
            return model;
        }

        public static TargetTimeModelTimes CreateTargetTimeModelTimes(Action<TargetTimeModelTimes>? setup = null)
        {
            var times = new TargetTimeModelTimes
            {
                TargetTimeModelId = 1,
                ValidFrom = DateTime.Today,
                Monday = 480,
                Tuesday = 480,
                Wednesday = 480,
                Thursday = 480,
                Friday = 480,
                Saturday = 0,
                Sunday = 0
            };
            setup?.Invoke(times);
            return times;
        }

        public static PersonTargetTimeModel CreatePersonTargetTimeModel(Action<PersonTargetTimeModel>? setup = null)
        {
            var ptm = new PersonTargetTimeModel
            {
                PersonId = 1,
                ValidFrom = DateTime.Today,
                TargetTimeModelId = 1
            };
            setup?.Invoke(ptm);
            return ptm;
        }

        public static List<Person> GetTestPersons(params Action<Person>[] setups)
        {
            var list = new List<Person>
            {
                CreatePerson(),
                CreatePerson(p =>
                {
                    p.Id = 2;
                    p.PersonnelNumber = "1002";
                    p.FirstName = "Erika";
                    p.LastName = "Musterfrau";
                })
            };
            for (int i = 0; i < setups.Length && i < list.Count; i++)
                setups[i]?.Invoke(list[i]);
            return list;
        }

        public static List<WorkingTime> GetTestWorkingTimes(params Action<WorkingTime>[] setups)
        {
            var list = new List<WorkingTime>
            {
                CreateWorkingTime(),
                CreateWorkingTime(w =>
                {
                    w.PersonId = 2;
                    w.Minutes = 420;
                })
            };
            for (int i = 0; i < setups.Length && i < list.Count; i++)
                setups[i]?.Invoke(list[i]);
            return list;
        }
    }
}