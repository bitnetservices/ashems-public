using System;
using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Catalog.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Salt { get; set; }

        public int TenantId { get; set; }

        public DateTime LastLoginTimestamp { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

    }
}
