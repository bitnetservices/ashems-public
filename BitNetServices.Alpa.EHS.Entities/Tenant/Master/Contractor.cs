using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class Contractor : MasterBaseEntity
    {
        public Contractor()
        {
            
        }

        [Key]
        public int ContractorId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string ManagerName { get; set; }
    }
}