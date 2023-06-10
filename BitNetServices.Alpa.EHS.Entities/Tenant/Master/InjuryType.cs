using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class InjuryType : MasterBaseEntity
    {
        public InjuryType()
        {

        }

        [Key]
        public int InjuryTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
