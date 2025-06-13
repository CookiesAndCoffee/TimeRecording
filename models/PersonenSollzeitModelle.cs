using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zeiterfassung.models
{
    public class PersonenSollzeitModelle
    {
        [Required]
        [Column(TypeName = "int")]
        public int PersonenId { get; set; }
        [ForeignKey(nameof(PersonenId))]
        public Person Person { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime GueltigAb { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int SollzeitModellId { get; set; }
        [ForeignKey(nameof(SollzeitModellId))]
        public SollzeitModelle SollzeitModell { get; set; }

        public PersonenSollzeitModelle() { }
    }
}
