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

		public virtual DbSet<Account> Accounts { get; set; }
		public virtual DbSet<Brand> Brands { get; set; }
		public virtual DbSet<Cart> Carts { get; set; }
		public virtual DbSet<Empolyee> Empolyees { get; set; }
		public virtual DbSet<Image> Images { get; set; }
		public virtual DbSet<MemberFollowList> MemberFollowLists { get; set; }
		public virtual DbSet<Member> Members { get; set; }
		public virtual DbSet<OrderItem> OrderItems { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<ProductCategory> ProductCategories { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<ProductSku> ProductSkus { get; set; }
		public virtual DbSet<SkuItem> SkuItems { get; set; }
		public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>()
				.Property(e => e.Account1)
				.IsUnicode(false);

			modelBuilder.Entity<Account>()
				.Property(e => e.Password)
				.IsUnicode(false);

			modelBuilder.Entity<Account>()
				.Property(e => e.Email)
				.IsFixedLength();

			modelBuilder.Entity<Brand>()
				.HasMany(e => e.Products)
				.WithRequired(e => e.Brand)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Empolyee>()
				.Property(e => e.Phone)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.Phone)
				.IsFixedLength();

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
				.HasMany(e => e.ProductSkus)
				.WithRequired(e => e.Product)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ProductSku>()
				.Property(e => e.Price)
				.HasPrecision(38, 2);

			modelBuilder.Entity<ProductSku>()
				.Property(e => e.Sale)
				.HasPrecision(38, 2);

			modelBuilder.Entity<ProductSku>()
				.HasMany(e => e.SkuItems)
				.WithRequired(e => e.ProductSku)
				.WillCascadeOnDelete(false);
		}
	}
}
