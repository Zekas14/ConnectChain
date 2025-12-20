using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectChain.Config.FluentApi
{

    public class ActivityCategoryConfiguration : IEntityTypeConfiguration<ActivityCategory>
    {
        public void Configure(EntityTypeBuilder<ActivityCategory> builder)
        {
            builder.ToTable("ActivityCategories");
            builder.HasKey(ac => ac.ID);
            builder.Property(ac => ac.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasMany(ac => ac.Suppliers)
                .WithOne(s => s.ActivityCategory)
                .HasForeignKey(s => s.ActivityCategoryId);
                



        }
    }
}
