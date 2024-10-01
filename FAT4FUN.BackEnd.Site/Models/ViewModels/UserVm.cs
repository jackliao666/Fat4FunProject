using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class UserVm
    {

        public int Id { get; set; }
        
        [Display(Name = "帳號")]
        public string Account { get; set; }
        
        [Display(Name = "姓名")]
        public string Name { get; set; }
        public bool Gender { get; set; }

        [Display(Name = "手機")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "狀態")]
        public bool Status { get; set; }

        [Display(Name = "權限")]
        public IEnumerable<string> Roles { get; set; }

       



    }
}