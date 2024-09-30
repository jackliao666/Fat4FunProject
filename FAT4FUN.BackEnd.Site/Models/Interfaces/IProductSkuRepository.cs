using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAT4FUN.BackEnd.Site.Models.Interfaces
{
    public interface IProductSkuRepository
    {
        List<Product2Dto> GetProduct();

        int Find(Product2Dto dto);
        void CreateSkuItems(SkuItemDto dto);
   
        List<ProductSku>GetProductSkus();

        Product2Dto GetProductId(int id);

        void UpdateSkuItems(SkuItemDto dto);

        //int Find2(Product2Dto dto);

        List<SkuItem> GetSkuItems(int id);
    }
}
