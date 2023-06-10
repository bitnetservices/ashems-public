using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class WorkPermitType : MasterBaseEntity
    {
        public WorkPermitType()
        {
            
        }
        [Key]
        public int WorkPermitTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}