using PreciseInVoice.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace PreciseInVoice.Domain.Entities
{
    public class User : BaseEntity
    {
        public string TenantId { get; set; } = null!;    

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;

        

        // ── Navigation ──
        public Tenant Tenant { get; set; } = null!;
    }
}