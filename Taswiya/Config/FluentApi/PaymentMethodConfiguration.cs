using ConnectChain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectChain.Config.FluentApi
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            // Table name
            builder.ToTable("PaymentMethods");

            // Primary key
            builder.HasKey(pm => pm.ID);

            // Properties
            builder.Property(pm => pm.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(pm => pm.Deleted)
                   .IsRequired();

            builder.Property(pm => pm.CreatedDate)
                   .IsRequired();

            builder.Property(pm => pm.UpdatedDate);

            // Many-to-Many relationship with Supplier through SupplierPaymentMethod
           /* builder.HasMany(pm => pm.U)
                   .WithMany(s => s.PaymentMethods)
                   .UsingEntity<SupplierPaymentMethod>(
                          join => join
                            .HasOne(spm => spm.Supplier)
                            .WithMany(s => s.SupplierPaymentMethods)
                            .HasForeignKey(spm => spm.SupplierID),
                           join => join
                            .HasOne(spm => spm.PaymentMethod)
                            .WithMany(pm => pm.SupplierPaymentMethods)
                            .HasForeignKey(spm => spm.PaymentMethodID),
                       join =>
                       {
                           join.HasKey(spm => new { spm.SupplierID, spm.PaymentMethodID });
                           join.ToTable("SupplierPaymentMethods");
                       });*/
        }
    }


}
