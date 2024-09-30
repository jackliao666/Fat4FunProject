using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class EditProfileDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
    }
}