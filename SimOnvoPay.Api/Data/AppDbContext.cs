using Microsoft.EntityFrameworkCore;
using SimOnvoPay.Api.Models;

namespace SimOnvoPay.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PaymentSession> PaymentSessions => Set<PaymentSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentSession>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Token).IsUnique();
            e.HasIndex(x => x.OnvoPaymentIntentId);
            e.HasIndex(x => x.ExternalReference);
            e.Property(x => x.Token).HasMaxLength(40).IsRequired();
            e.Property(x => x.Currency).HasMaxLength(3).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500).IsRequired();
            e.Property(x => x.CallbackUrl).HasMaxLength(2000).IsRequired();
            e.Property(x => x.SuccessUrl).HasMaxLength(2000).IsRequired();
            e.Property(x => x.CancelUrl).HasMaxLength(2000).IsRequired();
            e.Property(x => x.ExternalReference).HasMaxLength(200);
            e.Property(x => x.OnvoCustomerId).HasMaxLength(100);
            e.Property(x => x.OnvoPaymentMethodId).HasMaxLength(100);
            e.Property(x => x.OnvoPaymentIntentId).HasMaxLength(100);
            e.Property(x => x.PaymentMethodType).HasMaxLength(20);
            e.Property(x => x.ErrorMessage).HasMaxLength(1000);
            e.Property(x => x.MetadataJson).HasColumnType("json");
            e.Property(x => x.Status).HasConversion<string>();
        });
    }
}
