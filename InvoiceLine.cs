using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseInVoice.Domain.Entities
{
    public class InvoiceLine : BaseEntity
    {
        public string TenantId { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
        public string InvoiceId { get; set; } = null!;
        public string? ProductServiceId { get; set; } 


        // ─── Stored in Database (user input) ────────
        public  required string Description { get; set; }     // "Consultation strategie"
        public required decimal Quantity { get; set; }
        public required decimal UnitPriceHT { get; set; }                    // 500.00
        public required decimal TauxTVA { get; set; }                        // 19 (percentage)
        public int SortOrder { get; set; }                          // order of lines
        public decimal TotalHT { get; set; }                    
        public decimal MontantTVA { get; set; }
        public decimal TotalTTC { get; set; }


        // ─── Navigation ─────────────────────────────
        public Invoice Invoice { get; set; } = null!;
        public ProductService? ProductService { get; set; } 

    }
}
