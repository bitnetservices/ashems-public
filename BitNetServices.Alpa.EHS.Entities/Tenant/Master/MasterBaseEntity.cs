using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Catalog.Entities;
using BitNetServices.Alpa.EHS.Tenant.Entities;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{


    public class MasterBaseEntity : BaseEntity
    {
        public MasterBaseEntity() {
            IsEditable = true;
            IsActive = true;
            IsGlobal = false;
        }
        public bool IsActive { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsEditable { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string DisplayName { get; set; }
        //[Required]
        //[StringLength(50)]
        //public string ShortName { get; set; }
        //[Required]
        //[StringLength(50)]
        //public string ShortNamePlural { get; set; }

    }
}
