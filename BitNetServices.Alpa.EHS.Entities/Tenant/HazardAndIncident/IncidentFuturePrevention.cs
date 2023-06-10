using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class IncidentFuturePrevention : BaseEntity
    {
        public IncidentFuturePrevention()
        {

        }
        [Key]
        public int IncidentFuturePreventionId { get; set; }
        public int HazardIncidentId { get; set; }

        public HazardIncident HazardIncident { get; set; }

        public int FuturePreventionId { get; set; }
        public FuturePrevention FuturePrevention { get; set; }

    }
}
