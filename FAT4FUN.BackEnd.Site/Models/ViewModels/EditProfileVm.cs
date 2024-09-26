using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class EditProfileVm
    {
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "手機")]
        [StringLength(20)]
        public string Phone { get; set; }


        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }



        [Required]
        [Display(Name = "地址")]
        [StringLength(150)]
        public string Address { get; set; }
    }
}