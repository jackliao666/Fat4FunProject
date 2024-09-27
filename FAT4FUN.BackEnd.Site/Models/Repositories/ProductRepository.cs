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
using PagedList;
using System.Drawing.Printing;

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
                    ProductSkus = p.ProductSkus.Select(s => new ProductSkusDto
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
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
                    ProductSkus = p.ProductSkus.Select(s => new ProductSkusDto
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
                        Name = s.Name,
                        Price = s.Price,
                        Sale = s.Sale,

                    }).ToList(),
                }).FirstOrDefault(p => p.Id == id);

            return product;

        }


        public int Update(ProductDto productDto)
        {

            var product = _db.Products
                .Include(p => p.ProductSkus)
                .FirstOrDefault(p => p.Id == productDto.Id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            var product1 = WebApiApplication._mapper.Map<Product>(productDto);
            // 更新產品實體的屬性
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Status = productDto.Status;
            product.ModifyDate = productDto.ModifyDate; 
            
            // 保存更改至資料庫
            _db.SaveChanges();
            return product.Id;
        }

        public void UpdateSkus(ProductSkuDto productSkuDto)
        {
   
            // 確保 DTO 包含 Id
            if (productSkuDto.Id <= 0)
            {
                throw new ArgumentException("Product SKU ID must be greater than zero.", nameof(productSkuDto.Id));
            }

            // 從資料庫中查找現有的 SKU 實體
            var existingSku = _db.ProductSkus.Find(productSkuDto.Id);

            if (existingSku == null)
            {
                throw new Exception("Product SKU not found.");
            }

            // 將屬性從 DTO 更新到現有的 SKU 實體
            existingSku.Name = productSkuDto.Name;
            existingSku.Price = productSkuDto.Price;
            existingSku.Sale = productSkuDto.Sale;

            // 如果有其他需要更新的屬性，請在這裡添加

            // 標記實體為已修改
            _db.Entry(existingSku).State = System.Data.Entity.EntityState.Modified;

            // 保存更改
            _db.SaveChanges();
        }
        public void DeleteProductWithSkus(ProductDto productDto)
        {
            // 將 DTO 轉換回 Product 實體
           
            foreach (var skuDto in productDto.ProductSkus)
            {
                // 先從資料庫中查詢 SKU 實體
                var skuEntity = _db.ProductSkus.Find(skuDto.Id);
                if (skuEntity != null)
                {
                    _db.ProductSkus.Remove(skuEntity);
                }
            }
       
            var productEntity = _db.Products.Find(productDto.Id);
                if (productEntity == null)
                {
                    throw new Exception("Product not found.");
                }           

            // 刪除 Product
            _db.Products.Remove(productEntity);

            // 儲存變更到資料庫
            _db.SaveChanges();
        }

        public IPagedList<ProductDto> GetProductsByFilter(string categoryName, string brandName, string productName,  int page = 1, int pageSize = 10)
        {
            
            var products = GetProducts().ToList();

            if (!string.IsNullOrEmpty(categoryName))
            {
                products = products.Where(p => p.CategoryName.Contains(categoryName)).ToList();
            }
            if (!string.IsNullOrEmpty(brandName))
            {
                products = products.Where(p => p.BrandName.Contains(brandName)).ToList();
            }
            if (!string.IsNullOrEmpty(productName))
            {
                products = products.Where(p => p.Name.Contains(productName)).ToList();
            }
            //if (minPrice.HasValue)
            //{
            //    products = products.Where(p => p.Price >= minPrice.Value).ToList();
            //}
            //if (maxPrice.HasValue)
            //{
            //    products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            //}

            
            return products.OrderBy(p => p.Id).ToPagedList(page, pageSize);
        }

    }

}
