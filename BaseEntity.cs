using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseInVoice.Domain.Entities
{
    public abstract class BaseEntity
    {
        // ─── Primary Key ────────────────────────────
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // ─── Audit Info ─────────────────────────────
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } 
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
