using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class UserCheckDto
    {

        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public IEnumerable<int> Roles { get; set;}
    }
}