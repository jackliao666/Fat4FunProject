using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FAT4FUN.FrontEnd.Site.Models.EFModels
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<MemberFollowList> MemberFollowLists { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductSku> ProductSkus { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolesString> RolesStrings { get; set; }
        public virtual DbSet<SkuItem> SkuItems { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.No)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderItems)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Images)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.MemberFollowLists)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductSkus)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductSku>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.ProductSku)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductSku>()
                .HasMany(e => e.OrderItems)
                .WithRequired(e => e.ProductSku)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductSku>()
                .HasMany(e => e.SkuItems)
                .WithRequired(e => e.ProductSku)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.ConfirmCode)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MemberFollowLists)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
