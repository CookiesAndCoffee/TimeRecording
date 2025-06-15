using Zeiterfassung.models;

namespace Zeiterfassung.Test.models
{
    public static class PersonenSollzeitModelleTestData
    {
        public static List<PersonenSollzeitModelle> CreateTestData(List<Person> personen, List<SollzeitModelle> modelle)
        {
            return new List<PersonenSollzeitModelle>
            {
                new PersonenSollzeitModelle
                {
                    Person = personen[0],
                    PersonenId = personen[0].Id,
                    GueltigAb = new DateTime(2024, 1, 1),
                    SollzeitModell = modelle[0],
                    SollzeitModellId = modelle[0].Id
                },
                new PersonenSollzeitModelle
                {
                    Person = personen[1],
                    PersonenId = personen[1].Id,
                    GueltigAb = new DateTime(2024, 1, 1),
                    SollzeitModell = modelle[1],
                    SollzeitModellId = modelle[1].Id
                }
            };
        }
    }
}