using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class AreaOrBuilding : MasterBaseEntity
    {
        public AreaOrBuilding()
        {

        }

        [Key]
        public int AreaOrBuildingId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}