
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class Employee: MasterBaseEntity
    {
        public Employee()
        {
            
        }

        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int DepartmentId { get; set; }

        [StringLength(50)]
        public string Designation { get; set; }
        [StringLength(50)]
        public string Manager { get; set; }
        public int EmployeeTypeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateofJoining { get; set; }

        public String Email { get; set; }
        public bool IsUser { get; set; }
        public Guid UserId { get; set; }

        public Department Department { get; set; }
    }
}