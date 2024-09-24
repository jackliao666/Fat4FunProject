using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using System.Data;

namespace FAT4FUN.BackEnd.Site.Models.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository()
        {
            _db = new AppDbContext();
        }
        public List<ProductDto> GetProducts()
        {
            var products = _db.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Brand)
                .Include(p => p.ProductSkus)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    CategoryName = p.ProductCategory.CategoryName,
                    BrandName = p.Brand.Name,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    CreateDate = p.CreateDate,
                    ModifyDate = p.ModifyDate,
                    Specs = p.ProductSkus.Select(s => new ProductSkuDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Price = s.Price,
                        Sale = s.Sale,
                        SkuItems = s.SkuItems.Select(i => new SkuItemDto
                        {
                            Id = i.Id,
                            Key = i.key,
                            Value = i.value,
                            SkuPrice = i.Price
                        }).ToList(),
                    }).ToList(),
                }).ToList();


            return products;
        }
        public void Create(ProductDto dto)
        {
            // 查詢對應的 Category 和 Brand
            var category = _db.ProductCategories.FirstOrDefault(c => c.Id == dto.CategoryId);
            var brand = _db.Brands.FirstOrDefault(b => b.Id == dto.BrandId);

            if (category == null || brand == null)
            {
                throw new Exception("無效的類別或品牌");
            }

            // 將 ProductDto 映射為 Product，並加入 Category 和 Brand 名稱
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status,
                CreateDate = DateTime.Now, // 自動設置為現在時間
                ModifyDate = DateTime.Now, // 自動設置為現在時間
                ProductCategoryId = dto.CategoryId,
                BrandId = dto.BrandId,
                ProductSkus = dto.Specs.Select(s => new ProductSku
                {
                    Name = s.Name,
                    Price = s.Price,
                    Sale = s.Sale,
                    SkuItems = s.SkuItems.Select(i => new SkuItem
                    {
                        key = i.Key,
                        value = i.Value,
                        Price = i.SkuPrice
                    }).ToList()
                }).ToList(),
                // 加入類別名稱和品牌名稱
                ProductCategory = category,
                Brand = brand
            };

            // 新增資料
            _db.Products.Add(product);

            // 保存更改
            _db.SaveChanges();
        }


    }
}