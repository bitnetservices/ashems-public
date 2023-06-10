using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class HazardIncidentStage : MasterBaseEntity
    {
        public HazardIncidentStage()
        {
            
        }
        [Key]
        public int HazardIncidentStageId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}