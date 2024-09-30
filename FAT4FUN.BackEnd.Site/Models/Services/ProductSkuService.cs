using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FAT4FUN.BackEnd.Site.Models.Services
{
    public class ProductSkuService
    {
        private readonly IProductSkuRepository _repo;
        public ProductSkuService()
        {
            _repo = new ProductSkuRepository();
        }
        public ProductSkuService(IProductSkuRepository repo)
        {
            _repo = repo;
        }

        public List<Product2Vm> GetProducts()
        {
            var productDto = _repo.GetProduct();

            var ProductVm = productDto.Select(vm => new Product2Vm
            {
                Id = vm.Id,
                Name = vm.Name,
                ProductSkus = vm.ProductSkus.Select(s => new ProductSkuVm
                {
                    Id = s.Id,
                    ProductId = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    Sale = s.Sale,
                    SkuItems = s.SkuItems.Select(i => new SkuItemVm
                    {
                        Id = i.Id,
                        ProductSkuId = i.ProductSkuId,
                        Key = i.Key,
                        Value = i.Value,
                        Price = i.Price
                    }).ToList(),
                }).ToList(),
            }).ToList();
           


            return ProductVm;
        }
        public int Create(Product2Dto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Product2Dto cannot be null");
            }

            var productSkuId = _repo.Find(dto);

            var skuItemDto = new SkuItemDto
            {
                ProductSkuId = productSkuId,
                Key = dto.Key,
                Value = dto.Value,
                Price = dto.Price,
            };

            _repo.CreateSkuItems(skuItemDto);

            return dto.Id;
        }

        public List<SelectListItem> GetProductSkus()
        {
            var productskus = _repo.GetProductSkus();

            return productskus.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();
        }

        public Product2Vm GetProducts(int id)
        {
            var productDto = _repo.GetProductId(id);

            var productVm = WebApiApplication._mapper.Map<Product2Vm>(productDto);

            return productVm;
        }

        public int Update(Product2Dto dto)
        {
            var productSkuId = _repo.Find(dto);

            //var skuItemId = _repo.Find2(productSkuId);
            

            var skuItemDto = new SkuItemDto
            {
                Id = dto.SkuItemId,
                ProductSkuId = productSkuId,
                Key = dto.Key,
                Value = dto.Value,
                Price = dto.Price,
            };

            _repo.UpdateSkuItems(skuItemDto);

            return dto.Id;

        }

        public List<SelectListItem> GetSkuItems(int id)
        {
            var productskus = _repo.GetSkuItems(id);

            return productskus.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.value
            }).ToList();
        }

        public void Delete(int productId, int skuId, int skuItemId)
        {
            //var productDto = _repo.GetProductId(vm);


            //if (productDto != null)
            //{
            //    // 刪除所有與該產品相關的 SKUs
            //    var skuitem = _repo.GetProductId(vm);
            //    if (productDto != null)
            //    {
            //        // 呼叫 Repository 層刪除產品及其 SKU
            //        //_repo.DeleteProductSku(productDto);
            //    }
            //}

        }
    }
}