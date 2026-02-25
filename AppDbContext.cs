using Microsoft.EntityFrameworkCore;
using PreciseInVoice.Domain.Entities;
using System.Linq.Expressions;

namespace PreciseInVoice.Persistence
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
        public required DbSet<Tenant> Tenants { get; set; }

        public required DbSet<Client> Clients { get; set; }
        public required DbSet<Invoice> Invoices { get; set; }
        public required DbSet<InvoiceLine> InvoiceLines { get; set; }
        public required DbSet<ProductService> ProductServices { get; set; }
        public required DbSet<Payment> Payments { get; set; }
        public required DbSet<Reminder> Reminders { get; set; }
        public required DbSet<InvoiceSettings> InvoiceSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix ALL decimal properties in ALL entities at once
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)
                         || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(3);
            }

            //CE code évite l'affichage des entités supprimées (IsDeleted = true) dans les requêtes

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(
                            BuildSoftDeleteFilter(entityType.ClrType)
                        );
                }
            }

            
            // ─── Relations 1:1 ─────────────────────────
            

            // Tenant ↔ InvoiceSettings (1:1)
            modelBuilder.Entity<Tenant>()
                .HasOne(t => t.InvoiceSettings)
                .WithOne(s => s.Tenant)
                .HasForeignKey<InvoiceSettings>(s => s.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice ↔ Payment (1:1)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Payment)
                .WithOne(p => p.Invoice)
                .HasForeignKey<Payment>(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice ↔ Reminder (1:1)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Reminder)
                .WithOne(r => r.Invoice)
                .HasForeignKey<Reminder>(r => r.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            
            // ─── Relations 1:N ─────────────────────────
            

            // Tenant → Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tenant → Clients
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Tenant)
                .WithMany(t => t.Clients)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tenant → ProductServices
            modelBuilder.Entity<ProductService>()
                .HasOne(p => p.Tenant)
                .WithMany(t => t.ProductServices)
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tenant → Invoices
            // Restrict : on ne peut pas supprimer un tenant s'il a des factures
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Tenant)
                .WithMany(t => t.Invoices)
                .HasForeignKey(i => i.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Client → Invoices
            // Restrict : on ne peut pas supprimer un client s'il a des factures
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice → InvoiceLines
            // Cascade : supprimer une facture supprime ses lignes
            modelBuilder.Entity<InvoiceLine>()
                .HasOne(l => l.Invoice)
                .WithMany(i => i.Lines)
                .HasForeignKey(l => l.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // InvoiceLine → Tenant (pour filtrage global)
            modelBuilder.Entity<InvoiceLine>()
                .HasOne(l => l.Tenant)
                .WithMany()
                .HasForeignKey(l => l.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            // ProductService → InvoiceLines
            // Restrict : on ne peut pas supprimer un produit utilisé dans des factures
            modelBuilder.Entity<InvoiceLine>()
                .HasOne(l => l.ProductService)
                .WithMany(p => p.InvoiceLines)
                .HasForeignKey(l => l.ProductServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment → Tenant (pour filtrage global)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            // Reminder → Tenant (pour filtrage global)
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Tenant)
                .WithMany()
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            
            // ─── Indexes ───────────────────────────────
            

            // InvoiceNumber unique
            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNumber)
                .IsUnique();

            // Email User unique globalement
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Email Client unique par Tenant
            modelBuilder.Entity<Client>()
                .HasIndex(c => new { c.TenantId, c.Email });

            // Reference ProductService unique par Tenant
            modelBuilder.Entity<ProductService>()
                .HasIndex(p => new { p.TenantId, p.Reference });
        }

        // ─── Soft Delete Helper ─────────────────────
        private static LambdaExpression BuildSoftDeleteFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);
            return lambda;
        }

        // ─── Auto Audit (CreatedAt, UpdatedAt, DeletedAt) ───
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        if (entry.Entity.IsDeleted && entry.Entity.DeletedAt == default)
                        {
                            entry.Entity.DeletedAt = DateTime.UtcNow;
                        }
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


    }

    //model building c'est l'outil qui dit à EF comment construire les tables selon des instructions spécifiques 


    
}