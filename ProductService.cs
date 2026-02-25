using PreciseInVoice.Domain.Enums;


namespace PreciseInVoice.Domain.Entities
{
    public class ProductService : BaseEntity
    {
        // ─── What user fills in the form ────────────
        public ProductType Type { get; set; }                          // Produit or Service
        public required string Designation { get; set; }         // "Consultation strategie"
        public string? Description { get; set; }                       // optional description
        public string Reference { get; set; } = string.Empty;         // "REF-001"
        public required decimal PrixHT { get; set; }                            // 500.00
        public required decimal TauxTVA { get; set; }                           // 19
        public string? Unite { get; set; }                             // "jour", "heure", etc.
        //calculated fields
        public decimal MontantTVA { get; set; }         
        public decimal PrixTTC { get; set; }

        // ─── Belongs to Tenant ─────────────────────
        
        public string TenantId { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;

        //navigation
        // savoir quelles lignes de facture utilisent ce produit.
        // on ne peut pas supprimer un produit qui est utilisé dans une ligne de facture.
        public ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();



    }
}