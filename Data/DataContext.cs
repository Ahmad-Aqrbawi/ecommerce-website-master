using CP.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CP.API.Data
{
    public class DataContext : IdentityDbContext<Supplier,Role,int,IdentityUserClaim<int>,SupplierRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        // public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<PhotoForSupplier> PhotoForSuppliers { get; set; }
        public DbSet<PhotoForProduct> PhotoForProducts { get; set; }
        public DbSet<Section> Sections {get; set;}
        public DbSet<SocialCommunication> SocialCommunications {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SupplierRole>(
                supplierRole=>{
                    supplierRole.HasKey(sp=>new{sp.UserId,sp.RoleId});
                    supplierRole.HasOne(sp=>sp.Role)
                    .WithMany(r=>r.SupplierRoles)
                    .HasForeignKey(sr=>sr.RoleId)
                    .IsRequired();

                     supplierRole.HasOne(sp=>sp.Supplier)
                    .WithMany(r=>r.SupplierRoles)
                    .HasForeignKey(sr=>sr.UserId)
                    .IsRequired();
                }
            );

            builder.Entity<Customer>()
            .HasMany<Order>(o => o.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Payment>()
            .HasMany<Order>(p => p.Orders)
            .WithOne(p => p.Payment)
            .HasForeignKey(p => p.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Shipper>()
            .HasMany<Order>(s => s.Orders)
            .WithOne(s => s.Shipper)
            .HasForeignKey(s => s.ShipperId)
            .OnDelete(DeleteBehavior.Restrict);

            

            builder.Entity<Order>()
            .HasMany<OrderDetail>(d => d.OrderDetails)
            .WithOne(d => d.Order)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
              .HasMany<Section>(d => d.Sections)
              .WithOne(d => d.Category)
              .HasForeignKey(d => d.CategoryId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Supplier>()
                          .HasMany<PhotoForSupplier>(d => d.PhotoForSuppliers)
                          .WithOne(d => d.Supplier)
                          .HasForeignKey(d => d.SupplierId)
                          .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Product>()
            .HasMany<OrderDetail>(d => d.OrderDetails)
            .WithOne(d => d.Product)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

         
            builder.Entity<Section>()
            .HasMany<Product>(p => p.Products)
            .WithOne(p => p.Section)
            .HasForeignKey(p => p.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Supplier>()
            .HasMany<Product>(p => p.Products)
            .WithOne(s => s.Supplier)
            .HasForeignKey(s => s.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

             builder.Entity<Supplier>()
            .HasMany<PhotoForSupplier>(p => p.PhotoForSuppliers)
            .WithOne(s => s.Supplier)
            .HasForeignKey(s => s.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Product>()
            .HasMany<Payment>(p => p.Payments)
            .WithOne(s => s.Product)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
            .HasMany<PhotoForProduct>(p => p.PhotoForProducts)
            .WithOne(s => s.Product)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

         


        }


    }
}