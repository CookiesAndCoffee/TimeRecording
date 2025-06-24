using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeRecording.Models
{
    /// <summary>
    /// Represents a working time entry for a specific person and date.
    /// </summary>
    public class WorkingTime
    {
        [Required]
        [Column(TypeName = "int")]
        public int PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public int Minutes { get; set; }

        public WorkingTime() { }


        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// Two WorkingTime objects are considered equal if they have the same PersonId and Date.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is WorkingTime time &&
                   PersonId == time.PersonId &&
                   Date == time.Date;
        }



        /// <summary>
        /// Serves as the default hash function.
        /// Combines PersonId and Date for hash code generation.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(PersonId, Date);
        }
    }
}
