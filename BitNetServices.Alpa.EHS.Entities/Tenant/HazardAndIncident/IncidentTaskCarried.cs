using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class IncidentTaskCarried : BaseEntity
    {
        public IncidentTaskCarried()
        {

        }

        [Key]
        public int IncidentTaskCarriedId { get; set; }

        public int HazardIncidentId { get; set; }

        public HazardIncident HazardIncident { get; set; }

       
        public int TaskCarriedId { get; set; }
        public TaskCarried TaskCarried { get; set; }

    }
}
