using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class Consequence : MasterBaseEntity
    {
        public Consequence()
        {
            
        }
        [Key]
        public int ConsequenceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}