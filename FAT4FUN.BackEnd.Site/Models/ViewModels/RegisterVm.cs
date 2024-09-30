using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class RegisterVm
    {
        public int Id { get; set; }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(50)]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(64)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(64)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "手機")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "地址")]
        [StringLength(150)]
        public string Address { get; set; }

        [Display(Name = "性別")]
        public bool Gender { get; set; }

        [Display(Name = "角色")]
        [Required(ErrorMessage = "必須要給一個職位")]
        public int Roles { get; set; } 



    }

    public static class RegisterExt
    {
        public static User ToUser(this RegisterVm vm)
        {
            var salt = HashUtility.GetSalt();
            var hashPassword = HashUtility.ToSHA256(vm.Password, salt); //建立密碼的雜湊值
            var confirmCode = Guid.NewGuid().ToString("N"); //發送確認信時使用

            return new User
            {
                Id = vm.Id,
                Account = vm.Account,
                Password = hashPassword,
                Email = vm.Email,
                Name = vm.Name,
                Phone= vm.Phone,
                IsConfirmed = false, //新使用者預設狀態是: 未確認
                ConfirmCode = confirmCode,
                Gender = vm.Gender,
                Address = vm.Address,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Status = true, 
            };
        }

        public static RegisterDto ToDto(this RegisterVm vm)
        {
            return new RegisterDto
            {
                Id=vm.Id,
                Account= vm.Account,
                Password= vm.Password,
                Email= vm.Email,
                Name = vm.Name,
                Phone= vm.Phone,
                Address = vm.Address,
                Gender= vm.Gender,
                Roles = vm.Roles,
                

            };
        }

    }



}