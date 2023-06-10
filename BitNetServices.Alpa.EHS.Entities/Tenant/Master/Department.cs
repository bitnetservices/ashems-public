using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class Department: MasterBaseEntity
    {
        public Department()
        {

        }
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}