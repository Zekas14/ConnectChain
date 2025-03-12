using ConnectChain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectChain.Config.FluentApi.IdentityConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
           /* builder.HasData(new List<Role>
            {
                new()
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new()
                {

                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
            });*/
        }
    }
}
