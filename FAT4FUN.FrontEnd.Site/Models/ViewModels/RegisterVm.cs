using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
    public class RegisterVm
    {
        public int Id { get; set; }

        //[Display(Name = "帳號")]
        //[Required]
        //[StringLength(30)]
        public string Account { get; set; }

        /// <summary>
        /// 明碼
        /// </summary>
        //[Display(Name = "密碼")]
        //[Required]
        //[StringLength(50)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "確認密碼")]
        //[Required]
        //[StringLength(50)]
        //[Compare(nameof(Password))]
        //[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        //[Required]
        //[StringLength(256)]
        //[EmailAddress]
        public string Email { get; set; }

        //[Display(Name = "姓名")]
        //[Required]
        //[StringLength(30)]
        public string Name { get; set; }

        //[Display(Name = "手機")]
        //[Required]
        //[StringLength(10)]
        public string Phone { get; set; }

        //[Display(Name = "地址")]
        //[Required]
        //[StringLength(50)]
        public string Address { get; set; }

        //[Display(Name = "性別")]
        //[Required]
        public bool Gender { get; set; }

        public bool IsConfirmed { get; set; }
        public string ConfirmCode { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }

        // 加入新的屬性
        public bool IsConfirmCode { get; set; }
    }
}