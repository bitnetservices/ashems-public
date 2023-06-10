using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class WorkPermitStage : MasterBaseEntity
    {
        public WorkPermitStage()
        {
            
        }
        [Key]
        public int WorkPermitStageId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}