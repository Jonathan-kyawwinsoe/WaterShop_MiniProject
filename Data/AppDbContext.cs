using Microsoft.EntityFrameworkCore;
using water_shop.Entity;

namespace water_shop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Products> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Voucher> Vouchers { get; set; } = null!;
        public DbSet<VoucherDetail> VoucherDetails { get; set; } = null!;
        public DbSet<Delivery> Deliveries { get; set; } = null!;
        public DbSet<EmptyBottleTransaction> EmptyBottleTransactions { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // IsDeleted filter for soft delete //
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Delivery>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<Products>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Voucher>().HasQueryFilter(v => !v.IsDeleted);
            modelBuilder.Entity<VoucherDetail>().HasQueryFilter(vd => !vd.IsDeleted);

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(v => v.VoucherStatus)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });
            modelBuilder.Entity<Products>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.Customer)
                .WithMany(c => c.Vouchers)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.Delivery)
                .WithMany(d => d.Vouchers)
                .HasForeignKey(v => v.DeliveryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VoucherDetail>()
                .HasOne(vd => vd.Voucher)
                .WithMany(v => v.VoucherDetails)
                .HasForeignKey(vd => vd.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VoucherDetail>()
                .HasOne(vd => vd.Product)
                .WithMany(p => p.VoucherDetails)
                .HasForeignKey(vd => vd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmptyBottleTransaction>()
                .HasOne(ebt => ebt.Customer)
                .WithMany(c => c.EmptyBottleTransactions)
                .HasForeignKey(ebt => ebt.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmptyBottleTransaction>()
                .HasOne(eb => eb.Product)
                .WithMany(p => p.EmptyBottleTransaction)
                .HasForeignKey(eb => eb.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmptyBottleTransaction>()
                .HasOne(eb => eb.Voucher)
                .WithMany(v => v.EmptyBottleTransactions)
                .HasForeignKey(eb => eb.VoucherId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.UserName)
                .IsUnique();

        }
    }
}
