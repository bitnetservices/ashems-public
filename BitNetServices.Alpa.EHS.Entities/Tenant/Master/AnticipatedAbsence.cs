using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class AnticipatedAbsence : MasterBaseEntity
    {
        public AnticipatedAbsence()
        {
            
        }
        [Key]
        public int AnticipatedAbsenceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}