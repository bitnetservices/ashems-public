using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BitNetServices.Alpa.EHS.Tenant.Entities.Security
{
    public partial class SecurityRole:MasterBaseEntity
    {
        [Key]
        public int SecurityRoleId { get; set; }
        public string Name { get; set; }        
    }
}
