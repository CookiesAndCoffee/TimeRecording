using Zeiterfassung.models;

namespace Zeiterfassung.Test.models
{
    public static class SollzeitModelleZeitenTestData
    {
        public static List<SollzeitModelleZeiten> CreateTestData(List<SollzeitModelle> modelle)
        {
            return new List<SollzeitModelleZeiten>
            {
                new SollzeitModelleZeiten
                {
                    SollzeitModell = modelle[0],
                    SollzeitModellId = modelle[0].Id,
                    GueltigAb = new DateTime(2024, 1, 1),
                    Mo = 480, Di = 480, Mi = 480, Do = 480, Fr = 480, Sa = 0, So = 0
                },
                new SollzeitModelleZeiten
                {
                    SollzeitModell = modelle[1],
                    SollzeitModellId = modelle[1].Id,
                    GueltigAb = new DateTime(2024, 1, 1),
                    Mo = 240, Di = 240, Mi = 240, Do = 240, Fr = 240, Sa = 0, So = 0
                }
            };
        }
    }
}