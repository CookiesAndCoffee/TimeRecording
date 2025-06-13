using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zeiterfassung.models
{
    public class SollzeitModelle : Entity
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Modell { get; set; }

        public SollzeitModelle() { }
    }
}
