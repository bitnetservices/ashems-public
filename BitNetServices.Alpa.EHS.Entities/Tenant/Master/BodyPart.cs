using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class BodyPart: MasterBaseEntity
    {
        public BodyPart()
        {
            
        }
        [Key]
        public int BodyPartId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}