using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Infra
{
    public class HashUtility
    {

        public static string GetSalt()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Salt"];
        }


        public static string ToSHA256(string plainText, string salt = null)
        {
            salt = salt ?? GetSalt(); //若salt是null就取得GetSalt()的值

            using (var mySHA256 = SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(plainText + salt); //把密碼加上salt,並轉成byte[]
                var hash = mySHA256.ComputeHash(bytes); //進行SHA256加密

                var sb = new StringBuilder(); //用來存放加密字串
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("X2")); //把byte轉成16進為字串,字母是大寫
                }

                return sb.ToString(); //回傳加密後字串
            }

        }
    }
}