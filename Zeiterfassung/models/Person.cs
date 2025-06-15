using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zeiterfassung.models
{
    [Table("Personen")]
    public class Person : Entity
    {
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Personalnummer { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        public Person() { }
    }
}
