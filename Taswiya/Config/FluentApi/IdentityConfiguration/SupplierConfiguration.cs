using ConnectChain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectChain.Config.FluentApi.IdentityConfiguration
{

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(s => s.Id)
                .ValueGeneratedNever()
                .IsRequired();
        }
    }
}
