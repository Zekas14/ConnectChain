using ConnectChain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectChain.Config.FluentApi
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.Url)
                .IsRequired();
            builder.HasIndex(i=>i.Url);
        }
    }
    
}
