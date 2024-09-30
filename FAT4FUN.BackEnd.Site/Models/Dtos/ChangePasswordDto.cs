using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class ChangePasswordDto
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}