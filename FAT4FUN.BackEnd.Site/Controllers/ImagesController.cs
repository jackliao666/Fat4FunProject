using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class ImagesController : Controller
    {
        private readonly AppDbContext _db;
        public ImagesController()
        {
            _db = new AppDbContext();
        }

        public ActionResult Index(int? productId, int? imageId)
        {
            var model = new ImageVm();

            // 獲取所有產品的列表
            model.Products = _db.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            // 如果 productId 為空，設置為默認的第一個產品
            if (productId == null && model.Products.Count > 0)
            {
                productId = int.Parse(model.Products.FirstOrDefault()?.Value);
            }

            // 根據 productId 和 imageId 獲取當前產品的圖片
            if (imageId != null)
            {
                // 查找與該 productId 相關聯的特定 imageId 的圖片
                model.Images = _db.Images.Where(i => i.ProductId == productId && i.Id == imageId).OrderBy(i => i.Sort).ToList();
            }
            else
            {
                // 如果沒有 imageId，則返回 productId 相關的所有圖片
                model.Images = _db.Images.Where(i => i.ProductId == productId).OrderBy(i => i.Sort).ToList();
            }

            model.SelectedProductId = productId ?? 0; // 確保為選擇的產品ID賦值
            model.SelectedImageId = imageId ?? 0;     // 確保為選擇的圖片ID賦值

            return View(model);
        }


        [HttpGet]
        public ActionResult GetProductImages(int productId, int imageId)
        {
            // 根據 productId 和 imageId 從資料庫中查找圖片
            var image = _db.Images.FirstOrDefault(i => i.ProductId == productId && i.Id == imageId);

            if (image == null)
            {
                // 返回默認佔位圖片
                string defaultImagePath = Server.MapPath("~/Images/default.jpg");
                return File(defaultImagePath, "image/jpeg");
            }

            // 獲取圖片的物理路徑
            string fullPath = Server.MapPath(image.Path);

            if (!System.IO.File.Exists(fullPath))
            {
                // 如果圖片文件不存在，返回 404 或者錯誤
                return HttpNotFound("圖片文件不存在");
            }

            // 返回圖片文件
            return File(fullPath, "image/jpeg"); // 假設圖片是 JPEG 格式，根據實際格式調整
        }

        [HttpPost]
        public ActionResult UploadImage(int productId, int sort, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    // 定義前台專案的絕對路徑
                    string frontEndImagePath = @"C:\Users\Jack\Desktop\finalproject\Fat4FunProject\FAT4FUN.FrontEnd.Site\Images\";

                    string backEndImagePath = Server.MapPath("~/Images/");

                    if (!Directory.Exists(frontEndImagePath))
                    {
                        Directory.CreateDirectory(frontEndImagePath);
                    }

                    // 生成檔名，防止重名覆蓋
                    string fileName = Path.GetFileName(file.FileName);
                    string fullPath = Path.Combine(frontEndImagePath, fileName);

                    // 查找是否已經有相同 ProductId 和 Sort 的圖片記錄
                    var existingImage = _db.Images.FirstOrDefault(i => i.ProductId == productId && i.Sort == sort);

                    if (existingImage != null)
                    {
                        // 如果記錄已存在，先刪除舊圖片
                        string oldImagePath = Server.MapPath(existingImage.Path);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        // 更新資料庫中的路徑
                        existingImage.Path = "/Images/" + fileName;
                        existingImage.CreateDate = DateTime.Now; // 更新創建時間
                        _db.SaveChanges();
                    }
                    else
                    {
                        // 如果記錄不存在，創建新記錄
                        var newImageRecord = new Image
                        {
                            ProductId = productId,
                            Path = "/Images/" + fileName,
                            Sort = sort,
                            CreateDate = DateTime.Now
                        };
                        _db.Images.Add(newImageRecord);
                        _db.SaveChanges();
                    }

                    // 保存新圖片到前台 Images 目錄
                    file.SaveAs(fullPath);

                    // 返回成功響應，附帶圖片路徑
                    return Json(new { success = true, path = "/Images/" + fileName });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "未上傳檔案。" });
        }


    }
}
