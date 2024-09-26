using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using System.Data;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using System.Web.Configuration;

namespace FAT4FUN.BackEnd.Site.Models.Repositories
{
    public class ProductRepository:IProductRepository
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
                .OrderBy(p=>p.Id)
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
                    ProductSkus = p.ProductSkus.Select(s => new ProductSkuDto
                    {

                        Name = s.Name,
                        Price = s.Price,
                        Sale = s.Sale,
                        
                    }).ToList(),
                }).ToList();


            return products;
        }
       
        public int Create(ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "ProductDto cannot be null");
            }

            // 使用 AutoMapper 將 ProductDto 映射為 Product 實體
            var product = WebApiApplication._mapper.Map<Product>(productDto);

            // 手動設定 Product 的額外欄位
            product.ProductCategoryId = productDto.CategoryId;
            product.BrandId = productDto.BrandId;
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Status = productDto.Status;
            product.CreateDate = DateTime.Now;
            product.ModifyDate = DateTime.Now;

            // 新增 Product 到資料庫
            _db.Products.Add(product);
            _db.SaveChanges(); // 保存變更，讓資料庫生成 Product 的自動遞增 Id

            // 返回新創建的 Product Id
            return product.Id; // 返回新創建的 Product Id
        }

        
        public List<ProductCategory> GetCategories()
        {
            return _db.ProductCategories.ToList(); // 獲取所有分類
        }

        public List<Brand> GetBrands()
        {
            return _db.Brands.ToList(); // 獲取所有品牌
        }

        public void CreateSkus(ProductSkuDto productSkuDto)
        {
            var productSku = WebApiApplication._mapper.Map<ProductSku>(productSkuDto);

            // 添加到資料庫上下文
            _db.ProductSkus.Add(productSku);

            // 保存所有變更
            _db.SaveChanges();
        }

        public ProductDto GetProductId(int id)
        {
            var product = _db.Products
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
                    ProductSkus = p.ProductSkus.Select(s => new ProductSkuDto
                    {
                        Name = s.Name,
                        Price = s.Price,
                        Sale = s.Sale,

                    }).ToList(),
                }).FirstOrDefault(p => p.Id == id);

            return product;

        }

        public void Update(ProductDto ProductDto)
        {
            var product = _db.Products
        .Include(p => p.ProductSkus)
        .FirstOrDefault(p => p.Id == ProductDto.Id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // 更新產品實體的屬性
            product.Name = ProductDto.Name;
            product.Description = ProductDto.Description;
            product.Status = ProductDto.Status;
            product.ModifyDate = ProductDto.ModifyDate;
            product.ProductCategoryId = ProductDto.CategoryId;
            product.BrandId = ProductDto.BrandId;

            // 更新 SKU 的邏輯
            if (ProductDto.ProductSkus != null && ProductDto.ProductSkus.Any())
            {
                foreach (var skuDto in ProductDto.ProductSkus)
                {
                    var existingSku = product.ProductSkus.FirstOrDefault(s => s.Id == skuDto.ProductId);

                    if (existingSku != null)
                    {
                        // 更新現有 SKU 的屬性
                        existingSku.Name = skuDto.Name;
                        existingSku.Price = skuDto.Price;
                        existingSku.Sale = skuDto.Sale;
                    }
                    else
                    {
                        // 添加新的 SKU 到產品中
                        var newSku = new ProductSku
                        {
                            Name = skuDto.Name,
                            Price = skuDto.Price,
                            Sale = skuDto.Sale,
                            ProductId = product.Id
                        };

                        product.ProductSkus.Add(newSku);
                    }
                }
            }

            // 保存更改至資料庫
            _db.SaveChanges();
        }
    }
}