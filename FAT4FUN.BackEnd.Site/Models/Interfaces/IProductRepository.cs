using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAT4FUN.BackEnd.Site.Models.Interfaces
{
    public interface IProductRepository
    {
        List<ProductDto> GetProducts();
        int Create(ProductDto dto);
        void CreateSkus(ProductSkuDto productSkuDto);
        List<Brand> GetBrands();
        List<ProductCategory> GetCategories();

        ProductDto GetProductId(int id);
        int Update(ProductDto ProductDto);
        void UpdateSkus(ProductSkuDto productSkuDto);
        
    }
}
