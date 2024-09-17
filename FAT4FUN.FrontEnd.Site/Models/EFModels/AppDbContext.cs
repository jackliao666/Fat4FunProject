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

		public virtual DbSet<Brands> Brands { get; set; }
		public virtual DbSet<Images> Images { get; set; }
		public virtual DbSet<ProductCategories> ProductCategories { get; set; }
		public virtual DbSet<Products> Products { get; set; }
		public virtual DbSet<ProductSkus> ProductSkus { get; set; }
		public virtual DbSet<SkuItems> SkuItems { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Brands>()
				.HasMany(e => e.Products)
				.WithRequired(e => e.Brands)
				.HasForeignKey(e => e.BrandId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ProductCategories>()
				.HasMany(e => e.Products)
				.WithRequired(e => e.ProductCategories)
				.HasForeignKey(e => e.ProductCategoryId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Products>()
				.HasMany(e => e.Images)
				.WithRequired(e => e.Products)
				.HasForeignKey(e => e.ProductId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Products>()
				.HasMany(e => e.ProductSkus)
				.WithRequired(e => e.Products)
				.HasForeignKey(e => e.ProductId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ProductSkus>()
				.HasMany(e => e.SkuItems)
				.WithRequired(e => e.ProductSkus)
				.HasForeignKey(e => e.ProductSkuId)
				.WillCascadeOnDelete(false);
		}
	}
}
