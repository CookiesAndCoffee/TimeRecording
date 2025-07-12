using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeRecording.Models
{
    /// <summary>
    /// Represents a person entity with personnel number, first name, and last name.
    /// Inherits from <see cref="Entity"/> to provide a unique identifier.
    /// </summary>
    public class Person : Entity
    {
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string PersonnelNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        public Person() { }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({PersonnelNumber})";
        }
    }
}
