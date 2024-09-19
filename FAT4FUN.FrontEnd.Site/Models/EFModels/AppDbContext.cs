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
		public virtual DbSet<Image> Images { get; set; }
		public virtual DbSet<ProductCategory> ProductCategories { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<ProductSku> ProductSkus { get; set; }
		public virtual DbSet<SkuItem> SkuItems { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Brand>()
				.HasMany(e => e.Products)
				.WithRequired(e => e.Brand)
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
				.HasMany(e => e.SkuItems)
				.WithRequired(e => e.ProductSku)
				.WillCascadeOnDelete(false);
		}
	}
}
