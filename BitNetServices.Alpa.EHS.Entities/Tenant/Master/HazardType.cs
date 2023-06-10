using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class HazardType: MasterBaseEntity
    {

        public HazardType()
        { }
        [Key]
        public int HazardTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool IsOther { get; set; }
        public bool RequireDescription { get; set; }
    }

}