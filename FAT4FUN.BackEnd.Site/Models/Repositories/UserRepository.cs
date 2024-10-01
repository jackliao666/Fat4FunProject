using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;

namespace FAT4FUN.BackEnd.Site.Models.Repositories
{
    public class UserRepository : IUserRepository
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

        public void AssignRoleToUser(int userId, int roleId)
        {
            var userRole = new Role
            {
                UserId = userId,
                Role1 = roleId
            };

            _db.Roles.Add(userRole);
            _db.SaveChanges();
        }

        public int Create(RegisterDto dto)
        {
            // 轉換 DTO 為 User Entity
            var user = new User
            {
                Account = dto.Account,
                Password = dto.EncryptedPassword,
                Email = dto.Email,
                Name = dto.Name,
                Phone = dto.Phone,
                Gender = dto.Gender,
                Address = dto.Address,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                IsConfirmed = dto.IsConfirmed,
                ConfirmCode = dto.ConfirmCode,
                Status = true,
            };

            // 將使用者新增到資料庫
            _db.Users.Add(user);
            _db.SaveChanges();

            // 回傳使用者的 Id
            return user.Id;


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

        public void Update(UserDto dto)
        {

            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(dto.Id);
                if (user != null)
                {
                    // 手動更新需要更新的屬性
                    user.Name = dto.Name;
                    user.Phone = dto.Phone;
                    user.Email = dto.Email;
                    user.Address = dto.Address;
                    user.Password = dto.Password;

                    // 確保其他不應被修改的屬性保持原樣

                    db.SaveChanges();
                }
            }

        }

        public List<UserCheckDto> GetAllUsers()
        {
            var users = _db.Users
                .Include(x => x.Roles)
                .Select(x => new UserCheckDto
                {
                    Id = x.Id,
                    Account = x.Account,
                    Address = x.Address,
                    Name = x.Name,
                    Email = x.Email,
                    Gender = x.Gender,
                    Phone = x.Phone,
                    Status = x.Status,
                    Roles = x.Roles.Select(r => r.Role1)

                })
                .ToList();
            return users;
        }


        public void UpdateUserStatus(UserCheckDto dto)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(dto.Id);
                if (user != null)
                {
                    // 手動更新需要更新的屬性
                    user.Status = dto.Status;

                    // 確保其他不應被修改的屬性保持原樣

                    db.SaveChanges();
                }
            }
        }

        public User GetUserById(int id)
        {
            return _db.Users.FirstOrDefault(x => x.Id == id);
        }

        public bool GetStatus(string account)
        {
            return _db.Users.FirstOrDefault(x => x.Account == account).Status;
        }

        public void UpdateUserStatus(int userId, bool newStatus)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Status = newStatus;
                _db.SaveChanges();
            }
        }

        public void UpdateUserRole(int id, int newRole)
        {
            var userRole = _db.Roles.FirstOrDefault(r => r.UserId == id);
            if (userRole != null)
            {
                userRole.Role1 = newRole;
            }
            else
            {
                // 如果該使用者沒有角色，則新增
                _db.Roles.Add(new Role { UserId = id, Role1 = newRole });
            }

            _db.SaveChanges();
        }
    }
}