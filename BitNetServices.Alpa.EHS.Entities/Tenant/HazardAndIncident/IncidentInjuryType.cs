using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class IncidentInjuryType : BaseEntity
    {
        public IncidentInjuryType()
        {

        }

        [Key]
        public int IncidentInjuryTypeId { get; set; }
        
        public int HazardIncidentId { get; set; }
        public HazardIncident HazardIncident { get; set; }

        public int InjuryTypeId { get; set; }
        public InjuryType InjuryType { get; set; }

    }
}
