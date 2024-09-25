using FAT4FUN.BackEnd.Site.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAT4FUN.BackEnd.Site.Models.Interfaces
{
    public interface IUserRepository
    {
        void Active(int userId);
        void AssignRoleToUser(int userId, int role);
        int Create(RegisterDto dto);
        UserDto Get(int userId);
        UserDto Get(string account);
        bool IsAccountExist(string account);
    }

}
