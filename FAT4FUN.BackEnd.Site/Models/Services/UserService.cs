using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace FAT4FUN.BackEnd.Site.Models.Services
{
    public class UserService
    {
        private IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
           _repo = repo;
        }

        internal Result HandleLogin(LoginVm vm)
        {
            try
            {
                LoginDto dto = MvcApplication._mapper.Map<LoginDto>(vm);    
                Result validateResult = ValidateLogin(dto);
                return validateResult;
            }
            catch (Exception ex) 
            {
                return Result.Fail(ex.Message);
            }
        }

        private Result ValidateLogin(LoginDto dto)
        {
            UserDto user = _repo.Get(dto.Account);

        }
    }
}