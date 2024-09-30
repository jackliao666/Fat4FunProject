using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class ChangePasswordVm
    {
        public int Id { get; set; }

        [Display(Name = "原密碼")]
        [Required]
        [StringLength(64)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "新密碼")]
        [Required]
        [StringLength(64)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(64)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}