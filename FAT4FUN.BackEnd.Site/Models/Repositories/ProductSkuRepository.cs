using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using System.Runtime.InteropServices;

namespace FAT4FUN.BackEnd.Site.Models.Repositories
{
    public class ProductSkuRepository:IProductSkuRepository
    {
        private readonly AppDbContext _db;
        public ProductSkuRepository()
        {
             _db = new AppDbContext();
        }

        public List<Product2Dto> GetProduct()
        {
            var products = _db.Products
                .Include(s => s.ProductSkus)
                .OrderBy(s => s.Id)
                .Where(s => s.ProductSkus.Any(i => i.SkuItems.Any(x => !string.IsNullOrEmpty(x.value))))
                .Where(s=>s.Status)        
                .Select(s => new Product2Dto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ProductSkus = s.ProductSkus
                    .Where(i => i.SkuItems.Any(x => !string.IsNullOrEmpty(x.value)))
                    .Select(i => new ProductSkuDto
                    {
                        Id = i.Id,
                        ProductId = i.Id,
                        Name = i.Name,
                        Price = i.Price,
                        Sale = i.Sale,
                        SkuItems = i.SkuItems
                        .Where(x => !string.IsNullOrEmpty(x.value))
                        .Select(x => new SkuItemDto
                        {
                            Id = x.Id,
                            ProductSkuId = x.ProductSkuId,
                            Key = x.key,
                            Value = x.value,
                            Price = x.Price,

                        }).ToList(),
                    }).ToList(),
                }).ToList();
           

            return products;
        }

        public int Find(Product2Dto dto)
        {
            var productSkuId = _db.ProductSkus
             .Where(s => s.ProductId == dto.Id) 
             .Select(x => x.Id) 
             .FirstOrDefault();


            //var productSku = WebApiApplication._mapper.Map<ProductSku>(dto);


            return productSkuId;
        }

        public void CreateSkuItems(SkuItemDto dto)
        {
            var skuItem = WebApiApplication._mapper.Map<SkuItem>(dto);

            _db.SkuItems.Add(skuItem);
            _db.SaveChanges();
        }


        public List<ProductSku>GetProductSkus()
        {
            return _db.ProductSkus.
                OrderBy(x=>x.Name).ToList();
        }

        public Product2Dto GetProductId(int id)
        {
            var product = _db.Products
                .Include(s => s.ProductSkus)
                .OrderBy(s => s.Id)
                .Where(s => s.ProductSkus.Any(i => i.SkuItems.Any(x => !string.IsNullOrEmpty(x.value))))
                .Select(s => new Product2Dto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ProductSkus = s.ProductSkus
                    .Where(i => i.SkuItems.Any(x => !string.IsNullOrEmpty(x.value)))
                    .Select(i => new ProductSkuDto
                    {
                        Id = i.Id,                        
                        Name = i.Name,
                        Price = i.Price,
                        Sale = i.Sale,
                        SkuItems = i.SkuItems
                        .Where(x => !string.IsNullOrEmpty(x.value))
                        .Select(x => new SkuItemDto
                        {
                            Id = x.Id,
                            ProductSkuId = x.ProductSkuId,
                            Key = x.key,
                            Value = x.value,
                            Price = x.Price,

                        }).ToList(),
                    }).ToList(),
                }).FirstOrDefault(i=>i.Id ==id);


            return product;
        }

        public void UpdateSkuItems(SkuItemDto dto)
        {
            // 確保 DTO 包含 Id
            if (dto.Id <= 0)
            {
                throw new ArgumentException("Product SKU ID must be greater than zero.", nameof(dto.Id));
            }

            // 從資料庫中查找現有的 SKU 實體
            var existingSku = _db.SkuItems.Find(dto.Id);

            if (existingSku == null)
            {
                throw new Exception("Product SKUitem not found.");
            }

            // 將屬性從 DTO 更新到現有的 SKU 實體
            existingSku.value = dto.Value;
            existingSku.key = dto.Key;
            existingSku.Price = dto.Price;

            // 如果有其他需要更新的屬性，請在這裡添加

            // 標記實體為已修改
            _db.Entry(existingSku).State = System.Data.Entity.EntityState.Modified;

            // 保存更改
            _db.SaveChanges();
        }

        //public int Find2(Product2Dto dto)
        //{
        //    var skuItemId = _db.SkuItems
        //     .Where(s => s.Id == dto.ProductSku.SkuItems)
        //     .Select(x => x.Id)
        //     .FirstOrDefault();


        //    //var productSku = WebApiApplication._mapper.Map<ProductSku>(dto);


        //    return productSkuId;
        //}

        public List<SkuItem> GetSkuItems(int id)
        {
            var productSkuIds = _db.ProductSkus
             .Where(s => s.ProductId == id) 
             .Select(s => s.Id) 
             .ToList();

            
            var skuItems = _db.SkuItems
                .Where(s => productSkuIds.Contains(s.ProductSkuId)) 
                .ToList();

            return skuItems;
        }

        //public void DeleteProductSku(Product2Dto dto)
        //{

        //    foreach(var item in dto.ProductSku.SkuItems)
        //    {
        //       var skuitem = _db.SkuItems.Find(dto.ProductSku.SkuItems.Find(dto.ProductSku.SkuItems.))
        //    }
        //}
    }
}