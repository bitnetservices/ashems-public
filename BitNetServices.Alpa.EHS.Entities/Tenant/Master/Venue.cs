using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class Venue
    {
        public int VenueId { get; set; }
        [Required]
        [StringLength(50)]
        public string VenueName { get; set; }
        [Required]
        [StringLength(30)]
        public string VenueType { get; set; }
        [Required]
        [StringLength(128)]
        public string AdminEmail { get; set; }

        [Required]
        [StringLength(128)]
        public string AdminPassword { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; }


        public string Lock { get; set; }

        public virtual Countries CountryCodeNavigation { get; set; }
    }
}
