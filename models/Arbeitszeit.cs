using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zeiterfassung.models
{
    public class Arbeitszeit : Entity
    {
        [Required]
        [Column(TypeName = "int")]
        public int PersonenId { get; set; }
        [ForeignKey(nameof(PersonenId))]
        public Person Person { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Datum { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public int Minuten { get; set; }

        public Arbeitszeit() { }
    }
}
