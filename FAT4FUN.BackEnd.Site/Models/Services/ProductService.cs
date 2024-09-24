using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repo;

        public ProductService()
        {
            _repo = new ProductRepository();
        }
        // 獲取所有產品並轉換為 ViewModel
        public List<ProductVm> GetAll()
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
                Price = dto.Specs != null && dto.Specs.Any() ? dto.Specs.First().Price : 0, // 假設每個產品至少有一個規格
                Sale = dto.Specs != null && dto.Specs.Any() ? dto.Specs.First().Sale : 0,   // 假設使用第一個規格的價格
                Status = dto.Status,
                CreateDate = dto.CreateDate,
                ModifyDate = dto.ModifyDate
            }).ToList();

            return productsVm;
        }
        // 將 DTO 轉換為 ViewModel
        public ProductVm ConvertToVm(ProductDto dto)
        {
            var productVm = new ProductVm
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CategoryName = dto.CategoryName,
                BrandName = dto.BrandName,
                Status = dto.Status,
                CreateDate = dto.CreateDate,
                ModifyDate = dto.ModifyDate,
                // 根據需要，將其他 DTO 的屬性對應到 VM 屬性
            };

            return productVm;
        }
        // Create 方法
        public void Create(ProductVm vm)
        {
            // 將 ViewModel 轉回 DTO
            var dto = new ProductDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CategoryName = vm.CategoryName,
                BrandName = vm.BrandName,
                Status = vm.Status,
                CreateDate = vm.CreateDate,
                ModifyDate = vm.ModifyDate
            };

            // 呼叫 Repository 的 Create 方法
            _repo.Create(dto);
        }
    }
}