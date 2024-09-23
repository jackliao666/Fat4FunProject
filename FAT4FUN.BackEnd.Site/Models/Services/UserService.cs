using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Infra;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Services
{
    public class UserService
    {
        private IUserRepository _repo;
        public UserService() 
        {
            _repo = new UserRepository();
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
            _repo.Create(dto);

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
            bool isPasswordCorrect = (hashPassword.CompareTo(user.EncryptedPassword) == 0);

            //回傳
            return isPasswordCorrect ? Result.Success() : Result.Fail("帳號或密碼錯誤");

        }
    }

   
}