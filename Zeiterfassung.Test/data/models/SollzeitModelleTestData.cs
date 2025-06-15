using Zeiterfassung.models;

namespace Zeiterfassung.Test.models
{
    public static class SollzeitModelleTestData
    {
        public static List<SollzeitModelle> CreateTestData()
        {
            return new List<SollzeitModelle>
            {
                new SollzeitModelle { Id = 1, Modell = "Vollzeit" },
                new SollzeitModelle { Id = 2, Modell = "Teilzeit" }
            };
        }
    }
}