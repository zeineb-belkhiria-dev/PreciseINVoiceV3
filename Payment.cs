using PreciseInVoice.Domain.Enums;

namespace PreciseInVoice.Domain.Entities
{
    public class Payment : BaseEntity
    {
        // ─── What user fills in the modal ───────────
        public DateTime PaymentDate { get; set; }          // Date de paiement
        public PaymentMethod Method { get; set; }          // Mode de paiement
        public string? Reference { get; set; }             // Reference paiement (optional)

        // ─── From the invoice ───────────────────────
        public decimal Amount { get; set; }                // Montant (1,800.00 DT)




        // ─── Navigation ─────────────────────────────
        public string InvoiceId { get; set; } = null!;
        public Invoice Invoice { get; set; } = null!;
        public string TenantId { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;


    }
}