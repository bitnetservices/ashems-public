using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class IncidentResult : MasterBaseEntity
    {
        public IncidentResult()
        {
            
        }
        [Key]
        public int IncidentResultId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}