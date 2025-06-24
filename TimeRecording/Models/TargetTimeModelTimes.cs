using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeRecording.Models
{
    /// <summary>
    /// Represents the daily target times for a specific target time model, valid from a certain date.
    /// </summary>
    public class TargetTimeModelTimes
    {
        [Required]
        [Column(TypeName = "int")]
        public int TargetTimeModelId { get; set; }
        [ForeignKey(nameof(TargetTimeModelId))]
        public TargetTimeModel TargetTimeModel { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public int Monday { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Tuesday { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Wednesday { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Thursday { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Friday { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Saturday { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Sunday { get; set; }

        public TargetTimeModelTimes() { }

        /// <summary>
        /// Returns the target time in minutes for the specified date.
        /// </summary>
        /// <param name="date">The date for which to retrieve the target time.</param>
        /// <returns>The target time in minutes for the given day of the week.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the day of week is invalid.</exception>
        public int ForDate(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Monday => Monday,
                DayOfWeek.Tuesday => Tuesday,
                DayOfWeek.Wednesday => Wednesday,
                DayOfWeek.Thursday => Thursday,
                DayOfWeek.Friday => Friday,
                DayOfWeek.Saturday => Saturday,
                DayOfWeek.Sunday => Sunday,
                _ => throw new ArgumentOutOfRangeException(nameof(date), "Invalid Day of Week")
            };
        }
    }
}
