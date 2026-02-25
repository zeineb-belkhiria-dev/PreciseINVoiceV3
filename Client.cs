using PreciseInVoice.Domain.Enums;

namespace PreciseInVoice.Domain.Entities
{
    public class Client : BaseEntity
    {
        
        // ─── Client Info (from Nouveau client form) ─
        public required string RaisonSociale { get; set; }     
        public required string MatriculeFiscal { get; set; }

        // ── Contact Principal ──
        public required string ContactName { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string? WhatsappPhone { get; set; }

        //Client informations
        public required string Fax { get; set; }
        public string? Website { get; set; }                          

        // ─── Address ────────────────────────────────
        public string? Address { get; set; }                           
        public string? City { get; set; }                              
        public string? PostalCode { get; set; }                        
        public string Country { get; set; } = "Tunisie";              

        // ─── Other ──────────────────────────────────
        public string? Notes { get; set; }                             
        public ClientStatus Status { get; set; } = ClientStatus.Nouveau;

        public string TenantId { get; set; }=null!;
        public Tenant Tenant { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}
   