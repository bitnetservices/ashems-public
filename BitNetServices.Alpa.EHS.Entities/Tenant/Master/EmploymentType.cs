using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class EmploymentType : MasterBaseEntity
    {
        public EmploymentType()
        {
            
        }

        [Key]
        public int EmploymentTypeId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}