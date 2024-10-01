using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
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
        void Update(UserDto userIndb);
        List<UserCheckDto> GetAllUsers();
        void UpdateUserStatus(UserCheckDto userInDb );
        User GetUserById(int id);
        bool GetStatus(string account);
        void UpdateUserStatus(int userId, bool newStatus);
        void UpdateUserRole(int id, int newRole);
    }

}
