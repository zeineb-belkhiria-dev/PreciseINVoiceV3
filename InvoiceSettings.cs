using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseInVoice.Domain.Entities
{
    public class InvoiceSettings : BaseEntity
    {
        // ─── Numerotation des factures ───────────────
        public string Prefix { get; set; } = "FAC-";
        public string YearFormat { get; set; } = "YYYY";
        public int NextNumber { get; set; } = 1;

        // ─── Conditions de paiement ─────────────────
        public int DefaultPaymentDelay { get; set; } = 30;
        public string Currency { get; set; } = "DT (Dinar tunisien)";

        public decimal DefaultTauxTVA { get; set; } = 19m;
        public string? DefaultNotes { get; set; }


        // ─── Belongs to Tenant ─────────────────────
        
        public string TenantId { get; set; } = null!;

        // ── Navigation ──
        public Tenant Tenant { get; set; } = null!;




    }
}