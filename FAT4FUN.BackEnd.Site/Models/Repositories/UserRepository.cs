using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Repositories
{
    public class UserRepository:IUserRepository
    {
       private AppDbContext _db;

        public UserRepository()
        {
            _db = new AppDbContext();
        }
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Active(int userId)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new Exception($"User with ID {userId} not found.");
            }

            user.IsConfirmed = true;
            user.ConfirmCode = null;
            _db.SaveChanges();
        }

        public void Active(int userId, string confirmCode)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == userId && x.ConfirmCode == confirmCode);

            if (user == null)
            {
                throw new Exception("Invalid user ID or confirmation code.");
            }

            user.IsConfirmed = true;
            user.ConfirmCode = null;
            _db.SaveChanges();
        }



        public void Create(RegisterDto dto)
        {
            _db.Users.Add(new User
            {
                Account = dto.Account,
                Email = dto.Email,
                Password = dto.EncryptedPassword,
                Name = dto.Name,
                Phone = dto.Phone,
                ConfirmCode = dto.ConfirmCode,
                IsConfirmed = dto.IsConfirmed,
                Gender = dto.Gender,
                Address = dto.Address,
                CreateDate = DateTime.Now,  
                ModifyDate = DateTime.Now   

            });

            _db.SaveChanges();
        }

        public UserDto Get(string account)
        {
            var user = _db.Users.FirstOrDefault(x => x.Account == account);
            if (user == null) return null;

            return WebApiApplication._mapper.Map<UserDto>(user); 
        }

        public UserDto Get(int userId)
        {
            var user = _db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Account = user.Account,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                ConfirmCode = user.ConfirmCode,
                EncryptedPassword = user.Password,
                IsConfirmed = user.IsConfirmed,
                Address = user.Address
            };
        }

        public bool IsAccountExist(string account)
        {
            var user = _db.Users.AsNoTracking().FirstOrDefault(x => x.Account == account);
            return user != null;
        }
    }
}