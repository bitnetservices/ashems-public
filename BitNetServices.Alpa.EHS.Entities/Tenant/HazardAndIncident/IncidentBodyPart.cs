using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class IncidentBodyPart : BaseEntity
    {
        public IncidentBodyPart()
        {

        }
        [Key]
        public int IncidentBodyPartId { get; set; }


        public int BodyPartId { get; set; }

        public int HazardIncidentId { get; set; }
        public BodyPart BodyPart { get; set; }
        public HazardIncident HazardIncident { get; set; }

    }
}
