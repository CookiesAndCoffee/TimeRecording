using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zeiterfassung.models
{
    public class SollzeitModelleZeiten
    {
        [Required]
        [Column(TypeName = "int")]
        public int SollzeitModellId { get; set; }
        [ForeignKey(nameof(SollzeitModellId))]
        public SollzeitModelle SollzeitModell { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime GueltigAb { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public int Mo { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Di { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Mi { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Do { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Fr { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int Sa { get; set; }
        [Required]
        [Column(TypeName = "smallint")]
        public int So { get; set; }

        public SollzeitModelleZeiten() { }

        public int FürDatum(DateTime datum)
        {
            return datum.DayOfWeek switch
            {
                DayOfWeek.Monday => Mo,
                DayOfWeek.Tuesday => Di,
                DayOfWeek.Wednesday => Mi,
                DayOfWeek.Thursday => Do,
                DayOfWeek.Friday => Fr,
                DayOfWeek.Saturday => Sa,
                DayOfWeek.Sunday => So,
                _ => throw new ArgumentOutOfRangeException(nameof(datum), "Ungültiger Wochentag")
            };
        }
    }
}
