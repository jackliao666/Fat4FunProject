using FAT4FUN.BackEnd.Site.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAT4FUN.BackEnd.Site.Models.Interfaces
{
    internal interface IOrderRepository
    {
        OrderDto Get(int id);
        void Update(OrderDto orderInDb);
    }
}
