using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public partial class Countries
    {
        public Countries()
        {

        }

        [Key]
        [StringLength(3)]
        public string CountryCode { get; set; }
        [Required]
        [StringLength(50)]
        public string CountryName { get; set; }
        [Required]
        [StringLength(10)]
        public string Language { get; set; }

        public virtual ICollection<Venue> Venue { get; set; }

    }
}
