using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zeiterfassung.models
{
    public abstract class Entity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Entity entity && Id == entity.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
