

using Microsoft.EntityFrameworkCore;
using PreciseInVoice.Domain.Entities;
using PreciseInVoice.Domain.Enums;

namespace PreciseInVoice.Persistence
{
    public class DbInitializer
    {
        public async Task SeedData(AppDbContext context)
        {
            if (await context.Tenants.AnyAsync() ||
                await context.Users.AnyAsync() ||
                await context.Invoices.AnyAsync() ||
                await context.InvoiceLines.AnyAsync() ||
                await context.Payments.AnyAsync() ||
                await context.Clients.AnyAsync() ||
                await context.ProductServices.AnyAsync() ||
                await context.Reminders.AnyAsync() ||
                await context.InvoiceSettings.AnyAsync())
            {
                return;
            }



            var now = DateTime.UtcNow;
            const string seedUser = "system";

            // ─── Tenant ───
            var tenant = new Tenant
            {
                Name = "Tenant Sarra",
                RaisonSociale = "ABC SARL",
                MatriculeFiscal = "1234567/A/P/M/000",
                RegistreCommerce = "B0167890",
                Address = "45 Avenue Habib Bourguiba",
                City = "Tunis",
                PostalCode = "1000",
                Country = "Tunisie",
                Email = "contact@abc.tn",
                WhatsappPhone = "+216 98 765 432",
                Fax = "71 234 567",
                BankName = "BIAT",
                BankAgency = "Agence Lac",
                RIB = "08 200 0011 0000 1234 5678 90",
                IsActive = true
            };
            Stamp(tenant, seedUser, now);

            // ─── InvoiceSettings ───
            var invoiceSettings = new InvoiceSettings
            {
                TenantId = tenant.Id,
                Prefix = "FAC-",
                YearFormat = "YYYY",
                NextNumber = 1,
                DefaultPaymentDelay = 30,
                Currency = "DT",
                DefaultTauxTVA = 19m,
                DefaultNotes = "Merci pour votre confiance."
            };
            Stamp(invoiceSettings, seedUser, now);

            // ─── User ───
            var user = new User
            {
                TenantId = tenant.Id,
                FirstName = "Sarra",
                LastName = "Ben Ali",
                Email = "sarra@abc.tn",
                PasswordHash = "hashed_password",
                JobTitle = "Gérant",
                Phone = "+216 98 765 432",
                IsActive = true
            };
            Stamp(user, seedUser, now);

            // ─── Client ───
            var client = new Client
            {
                TenantId = tenant.Id,
                RaisonSociale = "XYZ Company",
                MatriculeFiscal = "7654321/B/P/M/000",
                ContactName = "Mohamed Ali",
                Email = "mohamed@xyz.tn",
                WhatsappPhone = "+216 22 333 444",
                Fax = "+216 71 987 654",
                Address = "10 Rue de Carthage",
                City = "Ariana",
                PostalCode = "2080",
                Country = "Tunisie",
                Status = ClientStatus.Actif
            };
            Stamp(client, seedUser, now);

            // ─── ProductService ───
            var product = new ProductService
            {
                TenantId = tenant.Id,
                Designation = "Consultation",
                Description = "Service de consultation",
                Reference = "SRV-001",
                PrixHT = 500m,
                TauxTVA = 19m,
                MontantTVA = 95m,
                PrixTTC = 595m,
                Type = ProductType.Service,
                Unite = "jour"
            };
            Stamp(product, seedUser, now);

            // ─── Invoice ───
            var invoice = new Invoice
            {
                TenantId = tenant.Id,
                ClientId = client.Id,
                InvoiceNumber = "FAC-2026-0001",
                IssueDate = now,
                DueDate = now.AddDays(30),
                Status = InvoiceStatus.Brouillon,
                SousTotalHT = 1000m,
                TotalTVA = 190m,
                TotalTTC = 1190m,
                Notes = "Première facture"
            };
            Stamp(invoice, seedUser, now);

            // ─── InvoiceLine ───
            var invoiceLine = new InvoiceLine
            {
                TenantId = tenant.Id,
                InvoiceId = invoice.Id,
                ProductServiceId = product.Id,
                Description = "Consultation - 2 jours",
                Quantity = 2m,
                UnitPriceHT = 500m,
                TauxTVA = 19m,
                SortOrder = 1,
                TotalHT = 1000m,
                MontantTVA = 190m,
                TotalTTC = 1190m
            };
            Stamp(invoiceLine, seedUser, now);

            // ─── Ajouter tout ───
            await context.Tenants.AddAsync(tenant);
            await context.InvoiceSettings.AddAsync(invoiceSettings);
            await context.Users.AddAsync(user);
            await context.Clients.AddAsync(client);
            await context.ProductServices.AddAsync(product);
            await context.Invoices.AddAsync(invoice);
            await context.InvoiceLines.AddAsync(invoiceLine);

            await context.SaveChangesAsync();
        }

        private static void Stamp(BaseEntity entity, string user, DateTime now)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = user;
            entity.IsDeleted = false;
        }
    }
}