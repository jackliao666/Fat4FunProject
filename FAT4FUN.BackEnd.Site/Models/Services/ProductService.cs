using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Models.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService()
        {
            _repo = new ProductRepository();
        }
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        

        // 獲取所有產品並轉換為 ViewModel
        public List<ProductVm> GetProducts()
        {
            // 從 Repository 獲取 ProductDto 列表
            var productsDto = _repo.GetProducts();

            // 將 ProductDto 列表映射為 ProductVm 列表
            var productsVm = productsDto.Select(dto => new ProductVm
            {
                Id = dto.Id,
                CategoryName = dto.CategoryName,
                BrandName = dto.BrandName,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.ProductSkus != null && dto.ProductSkus.Any() ? dto.ProductSkus.First().Price : 0, // 假設每個產品至少有一個規格
                Sale = dto.ProductSkus != null && dto.ProductSkus.Any() ? dto.ProductSkus.First().Sale : 0,
                Status = dto.Status,
                CreateDate = dto.CreateDate,
                ModifyDate = dto.ModifyDate
            }).ToList();

            return productsVm;
        }

        // Create 方法
        public int Create(ProductDto productDto)
        {
            // 確保 productDto 不為 null
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "ProductDto cannot be null");
            }

            // 創建 Product，並獲取生成的 Product Id
            var createdProductId = _repo.Create(productDto);

                var productskuDto = new ProductSkuDto
                {
                    ProductId = createdProductId,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Sale = productDto.Sale,
                    // 根據需要添加其他屬性
                };
           
                
                // 將 SKU 新增至資料庫
                // 呼叫 Repository 的 CreateSkus 方法
                _repo.CreateSkus(productskuDto);
            
            return createdProductId; // 返回新創建的 Product Id
        }
        public List<SelectListItem> GetCategories()
        {
            var categories = _repo.GetCategories(); // 從 Repository 獲取所有分類
            return categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList();
        }

        public List<SelectListItem> GetBrands()
        {
            var brands = _repo.GetBrands(); // 從 Repository 獲取所有品牌
            return brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
        }

        public ProductVm GetProduct(int id)
        {
            var productDto = _repo.GetProductId(id);
            
            var productVm = new ProductVm
            {
                Id = productDto.Id,
                CategoryName = productDto.CategoryName,
                BrandName = productDto.BrandName,
                Name = productDto.Name,
                Description = productDto.Description,
                ProductSkuName = productDto.ProductSkus != null && productDto.ProductSkus.Any() ? productDto.ProductSkus.First().Name : null, // 獲取第一個 SKU 的名稱
                Price = productDto.ProductSkus != null && productDto.ProductSkus.Any() ? productDto.ProductSkus.First().Price : 0, // 假設每個產品至少有一個規格
                Sale = productDto.ProductSkus != null && productDto.ProductSkus.Any() ? productDto.ProductSkus.First().Sale : 0,
                Status = productDto.Status,
                CreateDate = productDto.CreateDate,
                ModifyDate = productDto.ModifyDate
            };
            return productVm;

        }

        public void UpdateProduct(ProductDto productDto)
        {


            var createdProductId = _repo.Create(productDto);
            // 將 ViewModel 轉換為 DTO
            var productDto = new ProductDto
            {
                Id = vm.Id,
                CategoryId = vm.CategoryId,
                CategoryName = vm.CategoryName,
                BrandId = vm.BrandId,
                BrandName = vm.BrandName,
                Name = vm.Name,     
                Description = vm.Description,
                Status = vm.Status,      
                ModifyDate = DateTime.Now, // 更新修改時間
                ProductSkus = new List<ProductSkuDto>
                {
                    new ProductSkuDto // 使用 new 創建 ProductSkuDto 物件
                    {
                        ProductId = vm.Id, 
                        Name = vm.ProductSkus.ProductSkuName, 
                        Price = vm.ProductSkus.Price, 
                        Sale = vm.ProductSkus.Sale 
                    }
                }
            };

            _repo.Update(productDto);
        }
    }
}