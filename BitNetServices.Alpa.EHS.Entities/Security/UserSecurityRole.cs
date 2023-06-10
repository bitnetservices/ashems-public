using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BitNetServices.Alpa.EHS.Tenant.Entities.Security
{
    public partial class UserSecurityRole : MasterBaseEntity
    {
        [Key]
        public int UserSecurityRoleId { get; set; }

        public Guid SecurityRoleId { get; set; }
        public Guid UserId { get; set; }
    }
}
