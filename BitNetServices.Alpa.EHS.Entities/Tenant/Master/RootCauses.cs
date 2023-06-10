using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class RootCause : MasterBaseEntity
    {
        public RootCause()
        {
           
            CreatedOn = CreatedOn.Equals(System.DateTime.MinValue) ? System.DateTime.Today : CreatedOn;
            ModifiedOn = ModifiedOn.Equals(System.DateTime.MinValue) ? System.DateTime.Today : ModifiedOn;

        }
        [Key]
        public int RootCauseId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}