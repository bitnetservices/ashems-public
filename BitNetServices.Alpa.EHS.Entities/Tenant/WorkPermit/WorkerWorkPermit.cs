using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class WorkerWorkPermit : BaseEntity
    {
        public WorkerWorkPermit()
        {

        }
        [Key]
        public int WorkerWorkPermitId { get; set; }


        public int WorkPermitId { get; set; }

        public int WorkerId { get; set; }
        public Employee Worker { get; set; }
        public WorkPermit WorkPermit { get; set; }

    }
}
