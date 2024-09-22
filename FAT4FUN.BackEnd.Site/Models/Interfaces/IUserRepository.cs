using FAT4FUN.BackEnd.Site.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Interfaces
{
    public interface IUserRepository
    {
        UserDto Get(string account);
    }
}