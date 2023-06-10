using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            RowId = Guid.NewGuid();
        }
        
        public byte[] RowVersion { get; set; }
        public Guid RowId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }        
    }
}
