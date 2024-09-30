using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FAT4FUN.BackEnd.Site.Models.EFModels;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class ImageVm
    {
        public List<SelectListItem> Products { get; set; }
        public List<Image> Images { get; set; }
        public int SelectedProductId { get; set; }
        public int SelectedImageId { get; set; } // 用來保存選擇的圖片ID
    }
}