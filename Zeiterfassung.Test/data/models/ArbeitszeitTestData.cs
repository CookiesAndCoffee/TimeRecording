using Zeiterfassung.models;

namespace Zeiterfassung.Test.models
{
    public static class ArbeitszeitTestData
    {
        public static List<Arbeitszeit> CreateTestData(List<Person> personen)
        {
            return new List<Arbeitszeit>
            {
                new Arbeitszeit
                {
                    PersonenId = personen[0].Id,
                    Person = personen[0],
                    Datum = new DateTime(2024, 6, 3),
                    Minuten = 480
                },
                new Arbeitszeit
                {
                    PersonenId = personen[1].Id,
                    Person = personen[1],
                    Datum = new DateTime(2024, 6, 3),
                    Minuten = 240
                }
            };
        }
    }
}