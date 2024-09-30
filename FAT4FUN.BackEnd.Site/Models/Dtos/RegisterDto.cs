using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class RegisterDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
      
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ConfirmCode { get; set; }
        public string EncryptedPassword { get; set; }
        public bool IsConfirmed { get; set; }

        public string Address { get; set; }
        public bool Gender { get; set; }

        public int Roles { get; set; }
    }
}