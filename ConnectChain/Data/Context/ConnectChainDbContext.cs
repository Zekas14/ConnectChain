using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ConnectChain.Models;

namespace ConnectChain.Data.Context
{
    public class ConnectChainDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ConnectChainDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ActivityCategory> ActivityCategories { get; set; }
        public DbSet<RFQ> RFQs { get; set; }
        public DbSet<RfqAttachment> RfqAttachments { get; set; }
        public DbSet<RfqSupplierAssignment> RfqSupplierAssignments { get; set; }
        public DbSet <PaymentTerm> PaymentTerms { get; set; }
        public DbSet<Quotation> Quotations { get; set; }   

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ConnectChainDbContext).Assembly);

        }
       
    }
}
