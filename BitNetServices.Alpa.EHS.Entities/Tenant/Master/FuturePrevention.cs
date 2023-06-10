using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class FuturePrevention : MasterBaseEntity
    {
        public FuturePrevention()
        {
            
        }
        [Key]
        public int FuturePreventionId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}