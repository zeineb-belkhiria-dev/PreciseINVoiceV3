using PreciseInVoice.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PreciseInVoice.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        // ── Identité ──
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        /// ─── Legal Info ─────────────────────────────
        public required string RaisonSociale { get; set; }
        public required string MatriculeFiscal { get; set; }
        public required string RegistreCommerce { get; set; }

        // ─── Address ────────────────────────────────
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = "Tunisie";

        // ─── Contact ────────────────────────────────
        public string Email { get; set; }
        public string Fax { get; set; }
        public string WhatsappPhone { get; set; }
        public string? Website { get; set; }

        // ─── Branding ───────────────────────────────
        public string? LogoUrl { get; set; }

        // ─── Banking ────────────────────────────────
        public string? BankName { get; set; }
        public string? BankAgency { get; set; }
        public string? RIB { get; set; }



        // ─── Navigation ─────────────────────────────
        
        public InvoiceSettings? InvoiceSettings { get; set; }
        public ICollection<Client> Clients { get; set; } = new List<Client>();
        public ICollection<ProductService> ProductServices { get; set; } = new List<ProductService>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}