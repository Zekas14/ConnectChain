using ConnectChain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ConnectChain.Config.FluentApi.IdentityConfiguration
{

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {

            builder.Property(s => s.Id)
                .ValueGeneratedNever()
                .IsRequired();
            builder.HasMany(s => s.Products)
                   .WithOne(p => p.Supplier)
                   .HasForeignKey(p => p.SupplierId);
        }
    }
}
