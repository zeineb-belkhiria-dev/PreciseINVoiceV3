using PreciseInVoice.Domain.Enums;

namespace PreciseInVoice.Domain.Entities
{
    public class Invoice : BaseEntity
    {

        // ─── Navigation ─────────────────────────────
        public string TenantId { get; set; } = null!;               // who sends

        public string ClientId { get; set; } = null!;                // who receives
        

        // ─── Invoice Info (stored in database) ──────
        public required string InvoiceNumber { get; set; }   // FAC-2026-0028
        public required DateTime IssueDate { get; set; }                     // Date emission
        public required DateTime DueDate { get; set; }                      // Date echeance
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Brouillon;
        public string? Notes { get; set; }                          // Notes pour le client

        
        public decimal SousTotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }



        // ── Navigation ──
        public Tenant Tenant { get; set; } = null!;
        public Client Client { get; set; } = null!;
        public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
        public Payment Payment { get; set; } = null!;
        public Reminder? Reminder { get; set; }

    }
}
