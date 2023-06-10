using System.Text.RegularExpressions;

namespace BitNetServices.Alpa.EHS.Catalog.Entities
{
    public partial class Tenants
    {
        public byte[] TenantId { get; set; }
        public string TenantName { get; set; }
        public string ServicePlan { get; set; }
        public string RecoveryState { get; set; }

        public string TenantUrlDomain { get { return Regex.Replace(TenantName.ToLower(), @"\s", ""); } }

        public System.DateTime LastUpdated { get; set; }
    }
}
