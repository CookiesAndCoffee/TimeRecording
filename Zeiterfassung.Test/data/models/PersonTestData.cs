using Zeiterfassung.models;

namespace Zeiterfassung.Test.models
{
    public static class PersonTestData
    {
        public static List<Person> CreateTestData()
        {
            return new List<Person>
            {
                new Person { Id = 1, Personalnummer = "1001", Name = "Max Mustermann" },
                new Person { Id = 2, Personalnummer = "1002", Name = "Erika Musterfrau" },
                new Person { Id = 3, Personalnummer = "1003", Name = "John Doe" }
            };
        }
    }
}