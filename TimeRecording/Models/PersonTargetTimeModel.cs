using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeRecording.Models
{
    /// <summary>
    /// Represents the assignment of a target time model to a person, valid from a specific date.
    /// </summary>
    public class PersonTargetTimeModel
    {
        [Required]
        [Column(TypeName = "int")]
        public int PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int TargetTimeModelId { get; set; }
        [ForeignKey(nameof(TargetTimeModelId))]
        public TargetTimeModel TargetTimeModel { get; set; }

        public PersonTargetTimeModel() { }
    }
}
