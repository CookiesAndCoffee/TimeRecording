using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeRecording.Models
{
    /// <summary>
    /// Represents a target time model entity.
    /// Inherits from <see cref="Entity"/> to provide a unique identifier.
    /// </summary>
    public class TargetTimeModel : Entity
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Model { get; set; }

        public TargetTimeModel() { }

        public override string ToString()
        {
            return Model;
        }
    }
}
