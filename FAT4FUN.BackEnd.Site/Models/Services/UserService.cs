using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Infra;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Services
{
    public class UserService
    {
        private IUserRepository _repo;
        private readonly AppDbContext _db;
        public UserService() 
        {
            _repo = new UserRepository();
            _db = new AppDbContext();
        }

        public UserService (IUserRepository repo)
        {
            _repo = repo;
        }


        public void Register(RegisterDto dto)
        {
            //判斷是否帳號已存在
            bool isAccountExist = _repo.IsAccountExist(dto.Account);
            if (isAccountExist) throw new Exception("帳號已存在");

            // 建立新會員
            // 密碼進行雜湊
            string salt = HashUtility.GetSalt();
            string hasPassword = HashUtility.ToSHA256(dto.Password, salt);

            // 加入confirm code
            string confirmCode = Guid.NewGuid().ToString("N");

            //建立新會員
            dto.ConfirmCode = confirmCode;
            dto.EncryptedPassword = hasPassword;
            dto.IsConfirmed = false;

            int userId = _repo.Create(dto);

            // 保存角色到 Roles 表
            _repo.AssignRoleToUser(userId, dto.Roles);

            // todo 寄送驗證信


        }

        public void ActiveRegister(int userId, string confirmCode)
        {
            UserDto dto = _repo.Get(userId);
            if (dto == null) throw new Exception("無此使用者");
            if (dto.ConfirmCode != confirmCode) throw new Exception("驗證碼錯誤");
            if (dto.IsConfirmed.HasValue && dto.IsConfirmed.Value)
            {
                throw new Exception("使用者以啟用");
            }

            _repo.Active(userId);
        }

        public Result GetUserRole(string account)
        {
            //根據帳號查詢使用者
            var user  = _db.Users.FirstOrDefault(x => x.Account == account);
            if (user == null )
            {
                return Result.Fail("無此使用者");
            }

            //查詢該使用者權限
            var role =  _db.Roles.FirstOrDefault(r => r.UserId == user.Id);
            if (role == null)
            {
                return Result.Fail("此帳號無權限");
            }
            return Result.Success(role.Role1);
        }

        internal Result ValidateLogin(LoginDto dto)
        {
            //找出user
            UserDto user = _repo.Get(dto.Account);
            if(user == null) return Result.Fail("帳號或密碼錯誤");

            //判斷帳號是否開通
            if (!user.IsConfirmed.HasValue || user.IsConfirmed.Value == false)
            {
                return Result.Fail("使用者未開通");
            }

            //將密碼雜湊後比對
            string hashPassword = HashUtility.ToSHA256(dto.Password);
            bool isPasswordCorrect = (hashPassword.CompareTo(user.Password) == 0);

            //回傳
            return isPasswordCorrect ? Result.Success() : Result.Fail("帳號或密碼錯誤");

        }

        internal void AssignRole(int userId, int role)
        {
            var userRole = new Role
            {
                UserId = userId,
                Role1 = role
            };

            _db.Roles.Add(userRole);  // 將角色新增到資料庫
            _db.SaveChanges();
        }

        internal void UpdateProfile(EditProfileDto dto)
        {
            UserDto userIndb = _repo.Get(dto.Id);
            userIndb.Name = dto.Name;
            userIndb.Phone = dto.Phone;
            userIndb.Email = dto.Email;
            userIndb.Address = dto.Address;
            _repo.Update(userIndb);

        }

        internal void ChangePassword(string account, ChangePasswordDto dto)
        {
            UserDto userInDb = _repo.Get(account);
            string oldPassword = HashUtility.ToSHA256(dto.Password);
            string newPassword = HashUtility.ToSHA256(dto.NewPassword);

            if (oldPassword != userInDb.Password)
            {
                throw new Exception("密碼錯誤");
            }
            if (oldPassword == newPassword)
            {
                throw new Exception("新舊密碼不可相同");
            }
            if(dto.NewPassword != dto.ConfirmPassword)
            {
                throw new Exception("新密碼與確認密碼必須相同");
            }

            userInDb.Password = newPassword;
            
            _repo.Update(userInDb);
        }
    }

   
}