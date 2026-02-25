using PreciseInVoice.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PreciseInVoice.Domain.Entities
{
    public class Reminder : BaseEntity
    {
        // ─── What user fills in the modal ───────────
        public ReminderChannel reminderChannel { get; set; }   // Email, WhatsApp, SMS
        public ReminderType Type { get; set; }               // First, Second, Final
        public string Subject { get; set; } = string.Empty; // "Rappel - Facture FAC-2026-0026"
        public string Message { get; set; } = string.Empty; // "Bonjour, Nous vous rappelons..."
        public bool AttachPdf { get; set; } = true;            // Joindre le PDF
        public string? RecipientEmail { get; set; }           // contact@tech.tn

        // ─── Set automatically ──────────────────────
        public DateTime SentAt { get; set; }               // when the reminder was sent

 
        // ─── Navigation ─────────────────────────────
        public string InvoiceId { get; set; } = null!;  
        public Invoice Invoice { get; set; } = null!;

        public string TenantId { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;

    }
}