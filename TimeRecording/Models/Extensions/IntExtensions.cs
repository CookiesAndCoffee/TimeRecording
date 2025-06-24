using System.Globalization;

namespace TimeRecording.Models.Extensions
{
    /// <summary>
    /// Provides extension methods for integer values.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Converts minutes to industrial time format (e.g., 90 -> "1,50").
        /// </summary>
        /// <param name="minutes">The number of minutes.</param>
        /// <returns>The industrial time as a string (e.g., "1,50").</returns>
        public static string ToIndustryTime(this int minutes)
        {
            return (minutes / 60.0).ToString("0.00", CultureInfo.InvariantCulture).Replace('.', ',');
        }
    }
}
