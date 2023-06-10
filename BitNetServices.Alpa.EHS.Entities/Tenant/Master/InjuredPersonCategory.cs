using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class InjuredPersonCategory: MasterBaseEntity
    {
        public InjuredPersonCategory()
        {
        
        }

        [Key]
        public int InjuredPersonCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}