using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class Location: MasterBaseEntity
    {
        public Location()
        {
            
        }
        [Key]
        public int LocationId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}