using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class HazardStatus: MasterBaseEntity
    {
        public HazardStatus() { }
        [Key]
        public int HazardStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}