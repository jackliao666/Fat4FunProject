using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private AppDbcontext _db;

        public UserRepository()
        {
            _db = new AppDbcontext();
        }
        public UserRepository(AppDbcontext db)
        {
            _db = db;
        }

        public UserDto Get(string account)
        {
            throw new NotImplementedException();
        }
    }
}